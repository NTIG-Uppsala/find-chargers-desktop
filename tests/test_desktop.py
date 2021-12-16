#!/bin/python
import os
import pytest
import platform
import requests

url = "https://find-chargers.azurewebsites.net/"

data = {
    "address": "Testgatan 65",
    "coordinate_lat": 31.5,
    "coordinate_long": 21,
    "ac_1": 0,
    "ac_2": 0,
    "chademo": 1,
    "ccs": 1,
    "user_input": "Testing",
    "email_address": "test@test.com"
    }

#Check for output
def test_desktop_end_to_end():
    ret = os.popen('dotnet run --project="../desktop-app/find-chargers-desktop.csproj"')
    assert (ret.read().find("id"))

#Check for right version
def test_desktop_check_platorm():
    ret = os.popen('dotnet run --project="../desktop-app/find-chargers-desktop.csproj" --version')
    assert (ret.read().find(platform.system()))

def post_request():
    request_answer = requests.post(url+"post-charger", json=data).json()
    return request_answer["insertId"]

# Check charger by email
def test_check_charger_by_email():
    charger_id = post_request()
    ret = os.popen('dotnet run --project="../desktop-app/find-chargers-desktop.csproj" --email test@test.com')
    assert (ret.read().find(charger_id))

# Check charger in range
def test_check_charger_in_range():
    charger_id = post_request()
    ret = os.popen('dotnet run --project="../desktop-app/find-chargers-desktop.csproj" --lat 31.5 --long 21 --range 10000')
    assert (ret.read().find(charger_id))

# Check charger not in range
def test_check_charger_not_in_range():
    charger_id = post_request()
    ret = os.popen('dotnet run --project="../desktop-app/find-chargers-desktop.csproj" --lat 31 --long 21 --range 50')
    assert not (ret.read().find(charger_id))

def test_non_number_distance():
    expected_answer = {"errors": [{"value": "hej--","msg": "Must be a number!","param": "max_distance","location": "params"}]}
    ret = os.popen('dotnet run --project="../desktop-app/find-chargers-desktop.csproj" --lat 31 --long 21 --range hej--')
    assert ret.read().find(expected_answer)