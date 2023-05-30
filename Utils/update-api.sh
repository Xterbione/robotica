GNU nano 4.8                             update-webpanel.sh
#!/bin/sh
#stop service
echo "stopping api service..."
systemctl stop api.service
echo "done."
#clear folder for build location
echo "removing /var/api/* for new build"
rm -rf /var/api/*

#pull newest commit from main
echo "git reset hard for pull requist"
git reset --hard

echo "pulling..."
git pull
echo "building new api"
cd ../testAPI

#clear folder for build location
dotnet build -c Debug -o /var/api/
echo "restarting services"
#reload all services and start api service
sudo systemctl daemon-reload
sudo systemctl start api
