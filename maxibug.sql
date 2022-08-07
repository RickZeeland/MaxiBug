-- Heroku "Dataclips" SQL script

set transaction read write; 

CREATE TABLE project( id SERIAL PRIMARY KEY, datecreated TIMESTAMP with time zone, name VARCHAR(50), version VARCHAR(20) );

CREATE TABLE issues
(
  id SERIAL PRIMARY KEY,
  datecreated TIMESTAMP with time zone,
  datemodified TIMESTAMP with time zone,
  createdby VARCHAR(50),
  modifiedby VARCHAR(50),
  version VARCHAR(20),
  targetversion VARCHAR(20),
  priority smallint,
  status smallint,
  summary VARCHAR(200),
  description text,
  imagefilename VARCHAR(300),
  image_id integer,
);

CREATE TABLE tasks
(
  id SERIAL PRIMARY KEY,
  datecreated TIMESTAMP with time zone,
  datemodified TIMESTAMP with time zone,
  createdby VARCHAR(50),
  modifiedby VARCHAR(50),
  targetversion VARCHAR(20),
  priority smallint,
  status smallint,
  summary VARCHAR(50),
  description text,
);

CREATE TABLE users
(
  id SERIAL PRIMARY KEY,
  datecreated TIMESTAMP with time zone,
  name VARCHAR(50),
  description text,
  issuelock integer,
  tasklock integer,
);

CREATE TABLE images
(
  id SERIAL PRIMARY KEY,
  datecreated TIMESTAMP with time zone,
  name VARCHAR(256),
  data bytea,
);

