/****** -------------------------------------	Table Roles	---------------------------------- ******/

CREATE  TABLE Roles ( 
	id                   INT PRIMARY KEY IDENTITY(1,1),
	name_rol             VARCHAR(100)       
 ) ;

/****** -------------------------------------	Table Rol and departments for users	---------------------------------- ******/

CREATE  TABLE User_RolAndDepartment ( 
	id                   INT PRIMARY KEY IDENTITY(1,1),
	fk_id_rol            INT NOT NULL,
	fk_id_departament    INT NOT NULL,
	fk_id_user           INT NOT NULL      
 );

/****** -------------------------------------	Table Departments	---------------------------------- ******/

CREATE  TABLE Departaments ( 
	id                   INT PRIMARY KEY IDENTITY(1,1),
	name_departament     VARCHAR(100)       
 );

/****** -------------------------------------	Table Status	---------------------------------- ******/

CREATE TABLE User_Status ( 
	id                   INT PRIMARY KEY IDENTITY(1,1),
	name_status          VARCHAR(100)       
 ) ;

/****** -------------------------------------	Table Payment methods	---------------------------------- ******/

CREATE  TABLE Payment_Method ( 
	id                   INT PRIMARY KEY IDENTITY(1,1),
	name_paymentmethod   VARCHAR(100)       
 );

/****** -------------------------------------	Table user updates	---------------------------------- ******/

CREATE  TABLE User_Updates ( 
    id                   INT PRIMARY KEY IDENTITY(1,1),
	date_create          DATE  NOT NULL     ,
	date_update          DATE  NOT NULL     ,
	fk_user_create       INT   NOT NULL     ,
	id_updateuser        INT    
);

ALTER TABLE User_Updates ADD CONSTRAINT fk_User_Updates_users FOREIGN KEY ( fk_user_create ) REFERENCES Users( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

/****** -------------------------------------	Table Users	---------------------------------- ******/
CREATE TABLE Users (
    id                   INT PRIMARY KEY IDENTITY(1,1),
    name                 VARCHAR(100) NOT NULL,
    lastname             VARCHAR(100) NOT NULL,
    email                VARCHAR(100) NOT NULL,
    phone                VARCHAR(100),
    password             VARCHAR(100) NOT NULL,
    fk_id_status         INT NOT NULL,
    salary               VARCHAR(100),
    fk_id_paymentmethod  INT NOT NULL,
    CONSTRAINT AK_email UNIQUE(email)
);

/****** -------------------------------------	Alter Table for  Rol and departments for users	---------------------------------- ******/
ALTER TABLE User_RolAndDepartment ADD CONSTRAINT fk_User_RolAndDepartment_roles FOREIGN KEY ( fk_id_rol ) REFERENCES Roles( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE User_RolAndDepartment ADD CONSTRAINT fk_User_RolAndDepartment_depart FOREIGN KEY ( fk_id_departament ) REFERENCES Departaments( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE User_RolAndDepartment ADD CONSTRAINT fk_User_RolAndDepartment_users FOREIGN KEY ( fk_id_user ) REFERENCES Users( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

/****** -------------------------------------	Alter table for user updates	---------------------------------- ******/
ALTER TABLE User_Updates ADD CONSTRAINT fk_User_Updates_users FOREIGN KEY ( fk_user_create ) REFERENCES Users( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

/****** -------------------------------------	Alter Table for Users	---------------------------------- ******/
ALTER TABLE Users ADD CONSTRAINT fk_Users_status FOREIGN KEY (fk_id_status) REFERENCES User_Status (id) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE Users ADD CONSTRAINT fk_Users_payment_method FOREIGN KEY (fk_id_paymentmethod) REFERENCES Payment_Method (id) ON DELETE NO ACTION ON UPDATE NO ACTION;



/****** -------------------------------------	Status values	---------------------------------- ******/
INSERT INTO User_Status (name_status)
VALUES ('ACTIVE'), ('INACTIVE');

/****** -------------------------------------	Departments values	---------------------------------- ******/
INSERT INTO Departaments (name_departament)
VALUES ('Operations'), ('Administrative'), ('Accounting'), ('Human Resources');

/****** -------------------------------------	Roles values	---------------------------------- ******/
INSERT INTO Roles (name_rol)
VALUES ('Undefined'), ('Admin'), ('Employee'), ('Payroll Clerk');