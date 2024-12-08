# ChristmasTreeManager

## Hosting app with docker
1. enable ssh root login
    Access the sshd_config file using a text editor like nano: `sudo nano /etc/ssh/sshd_config`; change `PermitRootLogin` to `yes`.
2. Setup nginx with ssh
    apt-get update
    apt-get install nginx certbot -y
    cd /etc/nginx/sites-available/
    Create domain config file: nano yourdomain.de.conf
    Create certificate: openssl dhparam -out /etc/ssl/certs/dhparam.pem 4096
    systemctl stop nginx
    certbot certonly -d yourdomain.de
    ln -s /etc/nginx/sites-available/yourdomain.de.conf /etc/nginx/sites-enabled/
    nginx -t
    systemctl start nginx
3. Setup docker with app
    Install docker on linux Hosting:
     sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
     sudo systemctl enable docker.service
     sudo systemctl enable containerd.service
     sudo groupadd docker
     sudo usermod -aG docker <currentuser>
    Login in to docker container registry
    Pull image
    Create self signed certbot
     dotnet dev-certs https -ep ${HOME}/https/aspnetapp.pfx -p ctm