import json
import logging

from faker import Faker
from locust import HttpUser, task, run_single_user
from locust.env import Runner
from locust.env import Environment
from requests import RequestException
from logging import log
from config import Config
import requests as r


class UserBehavior(HttpUser):
    groups_count = 5
    secrets_count = 2
    cfg = Config()
    host = cfg['host']
    user_counter = 0
    groups = None

    @staticmethod
    def init_groups():
        groups = []
        for i in range(UserBehavior.groups_count):
            group = Faker().word()
            r.post(UserBehavior.cfg['admin_host'] + "/groups/create",
                   headers={'X-Api-Key': UserBehavior.cfg['admin_token']},
                   params={'name': group})
            groups.append({'name': group, 'secrets': []})

        return groups

    @staticmethod
    def init_secrets():
        for i in range(UserBehavior.groups_count):
            for j in range(UserBehavior.secrets_count):
                secretName = Faker().word()
                UserBehavior.groups[i]['secrets'].append(secretName)
                resp = r.post(f"{UserBehavior.cfg['admin_host']}/groups/{UserBehavior.groups[i]['name']}/add-secret",
                              headers={'X-Api-Key': UserBehavior.cfg['admin_token']},
                              json={'name': secretName, 'value': Faker().word()})
        return UserBehavior.groups

    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        if UserBehavior.groups is None:
            UserBehavior.groups = UserBehavior.init_groups()
            UserBehavior.init_secrets()
        self.user_number = UserBehavior.user_counter
        UserBehavior.user_counter += 1
        self.fake = Faker()
        self.headers = None
        self.refresh_token = None
        self.access_token = None
        self.occupied = None
        self.user_group_name = None
        self.secret_number = None
        self.group_number = None
        self.user = self.get_new_user()

    def on_start(self):
        response = self.client.post("/auth/login", json=self.user)
        self.access_token = response.json()["accessToken"]
        self.refresh_token = response.json()["refreshToken"]
        self.headers = {"Authorization": "Bearer " + self.access_token}
        self.occupied = False

    def on_stop(self):
        self.release()
        self.client.delete("/auth/logout", json=self.refresh_token, headers=self.headers)

    @task(1)
    def occupy(self):
        if not self.occupied and not self.check_busy():
            group = UserBehavior.groups[self.group_number]

            params = {'groupName': group['name'],
                      'secretName': group['secrets'][self.secret_number]}
            response = self.make_request(method="post", url="/secrets/occupy", headers=self.headers, params=params)

            if response.status_code == 200:
                self.occupied = True

    @task(1)
    def release(self):
        if self.occupied and self.check_busy():
            group = UserBehavior.groups[self.group_number]
            params = {'groupName': group['name'],
                      'secretName': group['secrets'][self.secret_number]}
            self.make_request(method="post", url="/secrets/release", headers=self.headers, params=params)
            self.occupied = False

    def get_new_user(self):
        user = {
            "clientId": self.fake.user_name(),
            "clientSecret": self.fake.password(),
        }

        response = r.post(UserBehavior.cfg['admin_host'] + "/auth", json=user,
                          headers={'X-Api-Key': UserBehavior.cfg['admin_token']})
        clientId = response.json()['clientId']

        self.group_number = self.user_number % UserBehavior.groups_count
        self.secret_number = (self.user_number // self.groups_count) % UserBehavior.secrets_count
        self.user_group_name = UserBehavior.groups[self.group_number]['name']
        try:
            resp = r.post(f"{UserBehavior.cfg['admin_host']}/groups/{self.user_group_name}/add-user",
                          headers={'X-Api-Key': UserBehavior.cfg['admin_token']},
                          params={'clientId': clientId})
            if resp.status_code != 200:
                log(msg=('Add user error:' + resp.text), level=logging.ERROR)
        except RequestException as e:
            log(level=logging.ERROR, msg=f"Error during HTTP request {e.request.url}: {e}")
        return user

    def check_busy(self):
        group = UserBehavior.groups[self.group_number]
        params = {'groupName': group['name'],
                  'secretName': group['secrets'][self.secret_number]}
        resp = self.make_request(url="/secrets/check", method='get', headers=self.headers, params=params)
        self.occupied = resp.json()
        return bool(resp.text)

    def make_request(self, method, url, **kwargs):
        try:
            response = getattr(self.client, method)(url, **kwargs)
            if response.status_code != 200:
                log(msg=('Add user error:' + response.text), level=logging.ERROR)
            return response
        except RequestException as e:
            log(level=logging.ERROR, msg=f"Error during HTTP request {url}: {e}")
            return None

    min_wait = 5000
    max_wait = 9000
