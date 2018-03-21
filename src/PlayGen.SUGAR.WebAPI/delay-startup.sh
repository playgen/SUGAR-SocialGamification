#!/bin/bash

if [ -z ${SUGAR_V0_MARIADB_PORT_3306_TCP} ]; then
	echo "You must link this container with sugar-v0-mariadb first"
	exit 1
fi
        
until nc -w 1 -z sugar-v0-mariadb 3306; do
	echo "$(date) - waiting for sugar-v0-mariadb..."
	sleep 1
done

exec "$@"
