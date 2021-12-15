#!/bin/python
import os
import pytest
import platform


#Check for output
def test_desktop_end_to_end():
    ret = os.popen('dotnet run --project="../desktop-app/find-chargers-desktop.csproj"')
    assert (ret.read() != "")

#Check for right version
def test_desktop_check_platorm():
    ret = os.popen('dotnet run --project="../desktop-app/find-chargers-desktop.csproj" --version')
    assert (ret.read().find(platform.system()))
