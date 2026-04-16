Create Database AdminLTE;
Use AdminLTE;

CREATE TABLE situacao (
  Id int NOT NULL AUTO_INCREMENT,
  Valor int DEFAULT '0',
  Texto varchar(1000) DEFAULT NULL,
  Parametro varchar(50) DEFAULT NULL,
  Observacao varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`)
);

INSERT INTO situacao VALUES (1,1,'Bloqueado','SituacaoPagamento',NULL),(2,2,'Liberado','SituacaoPagamento',NULL),(3,3,'Pendente','SituacaoPagamento',NULL);

CREATE TABLE usuarios (
  Id int NOT NULL AUTO_INCREMENT,
  Login varchar(100) DEFAULT NULL,
  Senha varchar(20) DEFAULT NULL,
  SenhaConfirmar varchar(20) DEFAULT NULL,
  Nome varchar(80) DEFAULT NULL,
  Celular varchar(15) DEFAULT NULL,
  Email varchar(120) DEFAULT NULL,
  Situacao int DEFAULT NULL,
  DataAlteracao datetime DEFAULT NULL,
  Supervisor tinyint(1) DEFAULT '0',
  TaxaPercentual double(5,2) DEFAULT NULL,
  DataCadastro datetime DEFAULT NULL,
  UltimoLogin datetime DEFAULT NULL,
  Observacao text,
  PRIMARY KEY (Id),
  KEY FK_IdSituacao_idx (Situacao),
  CONSTRAINT FK_Usuarios_Situacao_Id FOREIGN KEY (Situacao) REFERENCES situacao (Id)
);

INSERT INTO `usuarios` VALUES (1,'email@gmail.com','123456','123456','Usuario 1','(99) 99999-0000','email@gmail.com','2','2026-04-12 22:12:19',1,NULL,'2026-04-12 22:11:54','2026-04-12 22:11:54',NULL);
INSERT INTO `usuarios` VALUES (2,'email2@gmail.com','123456','123456','Usuario 2','(99) 99999-0000','email2@gmail.com','1','2026-04-12 22:12:19',1,NULL,'2026-04-12 22:11:54','2026-04-12 22:11:54',NULL);

