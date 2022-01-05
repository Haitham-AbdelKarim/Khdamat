
-- This is the script that creates the datbase objects and fills them

use Khdamat

create table Account
(Email varchar(50) PRIMARY KEY,
Password varchar(20) NOT NULL,
S_Blocked bit NOT NULL,
Admin_b bit,
Supporter_b bit,
Client_b bit,
Worker_b bit);

create table Administrator
(Natoinal_ID varchar(20) PRIMARY KEY,
Admin_Email varchar(50) NOT NULL,
F_Name varchar(20) NOT NULL,
L_Name varchar(20) NOT NULL,
Country varchar(20) NOT NULL,
City varchar(20) NOT NULL,
Street varchar(50),
Phone varchar(15) NOT NULL,
Gender char(1) NOT NULL,
FOREIGN KEY (Admin_Email) REFERENCES Account(Email)
ON DELETE CASCADE
ON UPDATE CASCADE);

create table Supporter
(Natoinal_ID varchar(20) PRIMARY KEY,
Supporter_Email varchar(50) NOT NULL,
F_Name varchar(20) NOT NULL,
L_Name varchar(20) NOT NULL,
Country varchar(20) NOT NULL,
City varchar(20) NOT NULL,
Street varchar(50),
Phone varchar(15) NOT NULL,
Gender char(1),
Birth_Date date NOT NULL,
Admin_ID varchar(20) NOT NULL,
FOREIGN KEY (Supporter_Email) REFERENCES Account(Email)
ON DELETE CASCADE
ON UPDATE CASCADE);

create table Client
(Natoinal_ID varchar(20) PRIMARY KEY,
Client_Email varchar(50) NOT NULL,
F_Name varchar(20) NOT NULL,
L_Name varchar(20) NOT NULL,
Country varchar(20) NOT NULL,
City varchar(20) NOT NULL,
Street varchar(50),
Phone varchar(15) NOT NULL,
Gender char(1),
Birth_Date date NOT NULL,
FOREIGN KEY (Client_Email) REFERENCES Account(Email)
ON DELETE CASCADE
ON UPDATE CASCADE);

create table Worker
(Natoinal_ID varchar(20) PRIMARY KEY,
Worker_Email varchar(50) NOT NULL,
F_Name varchar(20) NOT NULL,
L_Name varchar(20) NOT NULL,
Country varchar(20) NOT NULL,
City varchar(20) NOT NULL,
Street varchar(50),
Phone varchar(15) NOT NULL,
Gender char(1),
Birth_Date date NOT NULL,
Rating float DEFAULT 5,
FOREIGN KEY (Worker_Email) REFERENCES Account(Email)
ON DELETE CASCADE
ON UPDATE CASCADE);

create table Service
(Service_ID int PRIMARY KEY,
Name varchar(20) NOT NULL);

create table Request
(Req_ID int PRIMARY KEY,
Client_ID varchar(20) NOT NULL,
Title varchar(50) NOT NULL,
Description_Req varchar(8000),
Service_ID int NOT NULL,
Request_City varchar(20),
Min_Age int,
Max_Age int,
Min_Price int DEFAULT 0,
Supporter_ID varchar(20) NOT NULL,
Gender char(1),
Date_Req date NOT NULL,
Done bit DEFAULT 0,
FOREIGN KEY (Client_ID) REFERENCES Client
ON DELETE CASCADE
ON UPDATE CASCADE,
FOREIGN KEY (Service_ID) REFERENCES Service
ON DELETE CASCADE
ON UPDATE CASCADE);

create table Apply_Req
(Req_ID int,
Worker_ID varchar(20),
Comment varchar(1000),
Taken bit DEFAULT 0,
PRIMARY KEY (Req_ID,Worker_ID),
FOREIGN KEY (Req_ID) REFERENCES Request
ON DELETE CASCADE
ON UPDATE CASCADE,
FOREIGN KEY (Worker_ID) REFERENCES Worker
ON DELETE NO ACTION
ON UPDATE NO ACTiON);

create table Report
(Report_ID int PRIMARY KEY,
Supporter_ID varchar(20),
Description varchar(1000),
FOREIGN KEY (Supporter_ID) REFERENCES Supporter
ON DELETE SET NULL
ON UPDATE SET NULL);

create table Make_Report
(Report_ID int,
Worker_ID varchar(20),
Client_ID varchar(20),
PRIMARY KEY (Report_Id,Worker_ID,Client_ID),
FOREIGN KEY (Report_Id) REFERENCES Report
ON DELETE CASCADE
ON UPDATE CASCADE,
FOREIGN KEY (Worker_ID) REFERENCES Worker
ON DELETE no action
ON UPDATE no action,
FOREIGN KEY (Client_ID) REFERENCES Client
ON DELETE no action
ON UPDATE no action);

create table Review
(Review_ID int PRIMARY KEY,
Description varchar(1000),
Rating int DEFAULT 5);

create table Make_Review
(Review_ID int NOT NULL,
Client_ID varchar(20),
Req_ID int NOT NULL,
PRIMARY KEY (Review_ID,Req_ID,Client_ID),
FOREIGN KEY (Client_ID) REFERENCES Client
ON DELETE NO ACTION
ON UPDATE NO ACTION,
FOREIGN KEY (Req_ID) REFERENCES Request
ON DELETE NO ACTION
ON UPDATE NO ACTION,
FOREIGN KEY (Review_ID) REFERENCES Review
ON DELETE CASCADE
ON UPDATE CASCADE);

create table Complain_Suggestion
(ID int PRIMARY KEY,
Worker_ID varchar(20),
Client_ID varchar(20),
Supporter_ID varchar(20),
Descriptions varchar(1000) NOT NULL,
C_or_S char(1) NOT NULL,
FOREIGN KEY (Supporter_Id) REFERENCES Supporter
ON DELETE SET NULL
ON UPDATE CASCADE,
FOREIGN KEY (Worker_ID) REFERENCES Worker
ON DELETE NO ACTION
ON UPDATE NO ACTION,
FOREIGN KEY (Client_ID) REFERENCES Client
ON DELETE NO ACTION
ON UPDATE NO ACTION);

insert into Complain_Suggestion(ID,Descriptions,C_or_S)
values(1,'momo','s')

