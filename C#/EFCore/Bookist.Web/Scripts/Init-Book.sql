CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Book` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Cover` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Author` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `PubDate` date NOT NULL,
    `Description` varchar(2000) CHARACTER SET utf8mb4 NOT NULL,
    `Format` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `FetchUrl` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `FetchCode` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Book` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20211117074716_Init-Book', '6.0.0');

COMMIT;

