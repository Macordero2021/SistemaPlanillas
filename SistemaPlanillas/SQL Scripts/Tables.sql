/****** --------------------------------------------------------------------------------------- ******/

CREATE  TABLE Roles ( 
	id                   INT PRIMARY KEY IDENTITY(1,1),
	name_rol             VARCHAR(100)       
 ) ;

/****** --------------------------------------------------------------------------------------- ******/

CREATE  TABLE Rol_Departament_User ( 
	id                   INT PRIMARY KEY IDENTITY(1,1),
	fk_id_rol            INT NOT NULL,
	fk_id_departament    INT NOT NULL,
	fk_id_user           INT NOT NULL      
 );

ALTER TABLE rol_departament_user ADD CONSTRAINT fk_rol_departament_user_roles FOREIGN KEY ( fk_id_rol ) REFERENCES roles( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE rol_departament_user ADD CONSTRAINT fk_rol_departament_user FOREIGN KEY ( fk_id_departament ) REFERENCES departaments( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE rol_departament_user ADD CONSTRAINT fk_rol_departament_user_users FOREIGN KEY ( fk_id_user ) REFERENCES users( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

/****** --------------------------------------------------------------------------------------- ******/

CREATE  TABLE departaments ( 
	id                   INT PRIMARY KEY IDENTITY(1,1),
	name_departament     VARCHAR(100)       
 );

/****** --------------------------------------------------------------------------------------- ******/

CREATE TABLE Status_user ( 
	id                   INT PRIMARY KEY IDENTITY(1,1),
	name_status          VARCHAR(100)       
 ) ;

/****** --------------------------------------------------------------------------------------- ******/

CREATE  TABLE payment_method ( 
	id                   INT PRIMARY KEY IDENTITY(1,1),
	name_paymentmethod   VARCHAR(100)       
 );

/****** --------------------------------------------------------------------------------------- ******/

CREATE  TABLE update_users ( 
    id                   INT PRIMARY KEY IDENTITY(1,1),
	date_create          DATE  NOT NULL     ,
	date_update          DATE  NOT NULL     ,
	fk_user_create       INT  NOT NULL     ,
	id_updateuser        INT    
 );

ALTER TABLE update_users ADD CONSTRAINT fk_update_users_users FOREIGN KEY ( fk_user_create ) REFERENCES users( id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

/****** -------------------------------------	Tabla Users	---------------------------------- ******/
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
    CONSTRAINT AK_email UNIQUE(email),
    CONSTRAINT fk_users_status FOREIGN KEY (fk_id_status) REFERENCES Status_user (id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT fk_users_payment_method FOREIGN KEY (fk_id_paymentmethod) REFERENCES payment_method (id) ON DELETE NO ACTION ON UPDATE NO ACTION
);


/****** -------------------------------------	Status values	---------------------------------- ******/
INSERT INTO Status_user (name_status)
VALUES ('ACTIVO'), ('INACTIVO');

/****** -------------------------------------	Payment methos values	---------------------------------- ******/
INSERT INTO payment_method (name_paymentmethod)
VALUES ('EFECTIVO'), ('DEPOSITO');

/****** -------------------------------------	Departments values	---------------------------------- ******/
INSERT INTO departaments (name_departament)
VALUES ('ADMINISTRATIVE'), ('ACCOUNTING'), ('BODEGA');