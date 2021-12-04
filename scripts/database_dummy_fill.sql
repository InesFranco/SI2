USE Project1
GO;


BEGIN TRAN
	SET NOCOUNT ON

    INSERT INTO tipo_activo VALUES('coisa')
    INSERT INTO tipo_activo VALUES('coisa para outra coisa')

    INSERT INTO activo VALUES('piscina',GETDATE(),1, 'Apple', '7775xx', 1)
	INSERT INTO activo VALUES('bomba',GETDATE(),1, 'Jacksonville', '8888xy', 2)
    INSERT INTO activo VALUES('bomba',GETDATE(),1, 'Jacksonville', '8888xy', 2)
    INSERT INTO activo VALUES('bomba',GETDATE(),1, 'Jacksonville', '8888xy', 2)
    INSERT INTO activo VALUES('aquecedor',GETDATE(),1, 'Mcgrooves', '8866xy', 2)

    INSERT INTO activo_filho VALUES(1, 2)
    INSERT INTO activo_filho VALUES(1, 3)
    INSERT INTO activo_filho VALUES(1, 4)


    INSERT INTO funcionario VALUES(2345, 'Mikael Åkerfeldt', '1980-04-22', 'rua sao pedro', 'oficial das piscinas', 928181727, 'mccool@hotmail.com')
    INSERT INTO funcionario VALUES(4444, 'Thom York', '1970-05-31', 'rua sá', 'reparador piscinas', 987878787, 'anc@hotmail.com')
    INSERT INTO funcionario VALUES(6666, 'Elizabeth Fraser', '1967-04-22', 'rua sao pedro', 'oficial das piscinas', 928181727, 'mccool@hotmail.com')
    INSERT INTO funcionario VALUES(1111, 'Fiona Apple', '1980-05-31', 'rua sá', 'reparador piscinas', 987878787, 'anc@hotmail.com')

    INSERT INTO activo_gerente VALUES(1, 4)
    INSERT INTO activo_gerente VALUES(2, 2)

    INSERT INTO activo_preçocomercial VALUES(1, 1997, '2021-10-28')
    INSERT INTO activo_preçocomercial VALUES(1, 2019, '2021-10-22')
    INSERT INTO activo_preçocomercial VALUES(1, 1882, '2021-10-20')

    INSERT INTO competencia VALUES('arranjar piscinas')
    INSERT INTO competencia VALUES('drenar piscinas')



    INSERT INTO funcionario_competencia VALUES(1, 1)
    INSERT INTO funcionario_competencia VALUES(2, 2)
    INSERT INTO funcionario_competencia VALUES(3, 2)

    INSERT INTO equipa VALUES('Rua dos Sabes que mais', 2, 1)
    INSERT INTO equipa VALUES('Rua dos quem não sabem', 3, 2)

    INSERT INTO funcionario_equipa VALUES(1, 2)
    INSERT INTO funcionario_equipa VALUES(2, 2)
    INSERT INTO funcionario_equipa VALUES(2, 1)
    INSERT INTO funcionario_equipa VALUES(3, 2)
    INSERT INTO funcionario_equipa VALUES(4, 1)

    INSERT INTO intervencao VALUES(1,'arranjar piscinas', 'por atribuir', 200, '2021-04-30','2021-05-01')
    INSERT INTO intervencao VALUES(1,'drenar piscinas', 'concluido', 300, '2021-04-30', '2021-05-05')
    INSERT INTO intervencao VALUES(5,'arranjar aquecedor', 'em execução', 500, '2021-05-2', '2021-05-05')

    INSERT INTO intervencao_equipa VALUES(1, 1,'2021-04-30')
    INSERT INTO intervencao_equipa VALUES(1, 2,'2021-04-30')
    INSERT INTO intervencao_equipa VALUES(2, 3,'2021-04-30')

COMMIT
