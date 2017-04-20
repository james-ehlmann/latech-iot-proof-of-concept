
CREATE SEQUENCE devices_sequence
  START WITH 1
  INCREMENT BY 1;

-- database create script
create table devices(
	id int primary key not null default next value for devices_sequence,
	password varchar(25) not null,
	created timestamp not null,
)


CREATE SEQUENCE entries_sequence
  START WITH 1
  INCREMENT BY 1;

create table entries(
	id int primary key not null default next value for entries_sequence,
	device int not null,
	data varchar(25) not null,
	happened timestamp not null,
	FOREIGN KEY (device)
      REFERENCES devices(id)
)
