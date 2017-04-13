-- database create script
create table devices(
	id int primary key not null,
	password varchar(25) not null
)

create table entries(
	id int primary key not null,
	device int not null,
	data varchar(25) not null,

	FOREIGN KEY (device)
      REFERENCES devices(id)
)
commit