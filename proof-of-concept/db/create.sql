-- database create script
create table device(
	id int IDENTITY(1, 1) primary key not null,
	password varchar(25) not null,
	created timestamp not null,
)

create table entries(
	id int IDENTITY(1, 1) primary key not null,
	device int not null,
	data varchar(300) not null,
	happened timestamp not null,
	FOREIGN KEY (device)
      REFERENCES device(id)
)

create table individual(
	id int not null,
	device int not null,
	FOREIGN KEY (device)
		REFERENCES device(id),
	CONSTRAINT PK_individual 
		PRIMARY KEY (id, device)
)

create table fridge_day(
	id int IDENTITY(1, 1) primary key not null,
	device int not null,
	individual int not null,
	happened bigint not null,
	created timestamp not null,
	caloriesEaten int not null,
	constraint fridge_day_fk FOREIGN KEY (individual, device)
      REFERENCES individual(id, device)
)

create table fitbit_day(
	id int IDENTITY(1, 1) primary key not null,
	device int not null,
	individual int not null,
	created timestamp not null,
	happened bigint not null,
	steps int not null,
	caloriesOut int not null,
	totalDistance float not null,
	elevation int not null,
	averageHR float not null,
	totalMinutesAsleep int not null,
	totalSleepRecords int not null,
	totalTimeInBed int not null,
	constraint fitbit_day_fk FOREIGN KEY (individual, device)
      REFERENCES individual(id, device)
)

