-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`Usuarios`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Usuarios` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Usuarios` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(45) NULL,
  `apellido` VARCHAR(45) NULL,
  `email` VARCHAR(100) NOT NULL,
  `contraseña` VARCHAR(100) NULL,
  `telefono` VARCHAR(45) NULL,
  `tipo` ENUM('admin', 'cliente', 'recepcionista') NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Empleados`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Empleados` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Empleados` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(45) NULL,
  `tipo` VARCHAR(45) NULL,
  `apellido` VARCHAR(45) NULL,
  `telefono` VARCHAR(45) NULL,
  `email` VARCHAR(45) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Citas`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Citas` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Citas` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `usuario_id` INT UNSIGNED NOT NULL,
  `empleado_id` INT NOT NULL,
  `fecha` VARCHAR(45) NULL,
  `hora` VARCHAR(45) NULL,
  `estado` ENUM('pendiente', 'aprobada', 'finalizada', 'cancelada') NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Citas_Usuarios1_idx` (`usuario_id` ASC) VISIBLE,
  INDEX `fk_Citas_Empleados1_idx` (`empleado_id` ASC) VISIBLE,
  CONSTRAINT `fk_Citas_Usuarios1`
    FOREIGN KEY (`usuario_id`)
    REFERENCES `mydb`.`Usuarios` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Citas_Empleados1`
    FOREIGN KEY (`empleado_id`)
    REFERENCES `mydb`.`Empleados` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Servicios`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Servicios` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Servicios` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(45) NULL,
  `precio` DECIMAL(18,2) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Productos`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Productos` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Productos` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(245) NULL,
  `cantidad_existente` INT NULL,
  `categoria` ENUM('belleza', 'bebida', 'consumible') NULL,
  `precio` DECIMAL(18,2) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Facturas`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Facturas` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Facturas` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `cita_id` INT NOT NULL,
  `usuario_id` INT NOT NULL,
  `subtotal` DECIMAL(18,2) NULL,
  `total` DECIMAL(18,2) NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Facturas_Citas1_idx` (`cita_id` ASC) VISIBLE,
  INDEX `fk_Facturas_Usuarios1_idx` (`usuario_id` ASC) VISIBLE,
  CONSTRAINT `fk_Facturas_Citas1`
    FOREIGN KEY (`cita_id`)
    REFERENCES `mydb`.`Citas` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Facturas_Usuarios1`
    FOREIGN KEY (`usuario_id`)
    REFERENCES `mydb`.`Usuarios` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Pagos`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Pagos` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Pagos` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `cuenta` VARCHAR(225) NULL,
  `monto` DECIMAL(18,2) NULL,
  `metodo_de_pago` ENUM('transferencia', 'tarjeta', 'efectivo') NULL,
  `nota` VARCHAR(45) NULL,
  `factura_id` INT NOT NULL,
  `usuario_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Pagos_Usuarios1_idx` (`usuario_id` ASC) VISIBLE,
  INDEX `fk_Pagos_Facturas1_idx` (`factura_id` ASC) VISIBLE,
  CONSTRAINT `fk_Pagos_Usuarios1`
    FOREIGN KEY (`usuario_id`)
    REFERENCES `mydb`.`Usuarios` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pagos_Facturas1`
    FOREIGN KEY (`factura_id`)
    REFERENCES `mydb`.`Facturas` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Facturas_detalles`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Facturas_detalles` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Facturas_detalles` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `factura_id` INT NOT NULL,
  `producto_id` INT NULL,
  `servicio_id` INT NULL,
  `cantidad` INT NULL,
  `precio unitario` DECIMAL(18,2) NULL,
  `total` DECIMAL(18,2) NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Producto_detalles_Productos_idx` (`producto_id` ASC) VISIBLE,
  INDEX `fk_Producto_detalles_Facturas1_idx` (`factura_id` ASC) VISIBLE,
  INDEX `fk_Facturas_detalles_Servicios1_idx` (`servicio_id` ASC) VISIBLE,
  CONSTRAINT `fk_Producto_detalles_Productos`
    FOREIGN KEY (`producto_id`)
    REFERENCES `mydb`.`Productos` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Producto_detalles_Facturas1`
    FOREIGN KEY (`factura_id`)
    REFERENCES `mydb`.`Facturas` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Facturas_detalles_Servicios1`
    FOREIGN KEY (`servicio_id`)
    REFERENCES `mydb`.`Servicios` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
