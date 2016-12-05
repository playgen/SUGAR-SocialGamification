#!/bin/bash

if [ -z ${SUGAR_MARIADB_PORT_3306_TCP} ]; then
	echo "You must link this container with mariadb first"
	exit 1
fi
        
until nc -z sugar-mariadb 3306; do
	echo "$(date) - waiting for mariadb..."
	sleep 1
done

exec "$@"
