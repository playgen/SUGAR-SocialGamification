#!/bin/bash

if [ -z ${SUGAR_MARIADB_PORT_3306_TCP} ]; then
	echo "You must link this container with mariadb first"
	exit 1
fi
        
until nc -w 1 -z sugar-mariadb 3306; do
	echo "$(date) - waiting for sugar-mariadb..."
	sleep 1
done

exec "$@"
