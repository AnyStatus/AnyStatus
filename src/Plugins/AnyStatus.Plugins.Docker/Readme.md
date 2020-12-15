### Docker Desktop Endpoint

npipe://./pipe/docker_engine

### Enable Docker API

1. Edit file /lib/systemd/system/docker.service
2. ExecStart=/usr/bin/docker daemon -H fd:// -H tcp://0.0.0.0:2375
3. sudo service docker restart
4. curl http://localhost:2375/version
