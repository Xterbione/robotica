#!/bin/sh
#stop service
systemctl stop api.service

#clear folder for build location
rm -rf /var/api/*

#pull newest commit from main
git pull

#clear folder for build location
dotnet build -c Debug -o /var/api/

#reload all services and start api service
sudo systemctl daemon-reload
sudo systemctl start api