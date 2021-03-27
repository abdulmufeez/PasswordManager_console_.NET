--Creating database for applicaton
DROP DATABASE IF EXISTS "pwManager";
CREATE DATABASE "pwManager";
\c pwManager;

--creating table for superuser
CREATE TABLE superuser(
    id SERIAL PRIMARY KEY NOT NULL, --- serial is constraint for int with auto increment
    user_name VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(100) NOT NUll,
    email VARCHAR(100) NOT NULL
);

--creating table for user record
CREATE TABLE users_data (
    id SERIAL PRIMARY KEY NOT NULL,
    website VARCHAR(100) NOT NULL,
    user_name VARCHAR(100) NOT NULL,
    password VARCHAR(100) NOT NUll,
    email VARCHAR(100) DEFAULT(NULL),
    url VARCHAR(150) DEFAULT(NULL),
    superuser_id INT NOT NULL,
    FOREIGN KEY(superuser_id) REFERENCES superuser(id)
	  ON DELETE CASCADE
);