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

    INSERT INTO activo_filho VALUES(1, 2)
    INSERT INTO activo_filho VALUES(1, 3)
    INSERT INTO activo_filho VALUES(1, 4)


    INSERT INTO funcionario VALUES(2345, 'Mikael Åkerfeldt', '1980-04-22', 'rua sao pedro', 'oficial das piscinas', 928181727, 'mccool@hotmail.com')
    INSERT INTO funcionario VALUES(4444, 'Thom York', '1970-05-31', 'rua sá', 'reparador piscinas', 987878787, 'anc@hotmail.com')
    INSERT INTO funcionario VALUES(6666, 'Elizabeth Fraser', '1967-04-22', 'rua sao pedro', 'oficial das piscinas', 928181727, 'mccool@hotmail.com')
    INSERT INTO funcionario VALUES(1111, 'Fiona Apple', '1980-05-31', 'rua sá', 'reparador piscinas', 987878787, 'anc@hotmail.com')




    INSERT INTO activo_gerente VALUES(1, 1)
    INSERT INTO activo_gerente VALUES(2, 2)

    INSERT INTO activo_preçocomercial VALUES(1, 1997, '2021-10-28')
    INSERT INTO activo_preçocomercial VALUES(1, 2019, '2021-10-22')
    INSERT INTO activo_preçocomercial VALUES(1, 1882, '2021-10-20')

    INSERT INTO competencia VALUES('arranjador de piscinas')
    INSERT INTO competencia VALUES('criador de piscinas')



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

    INSERT INTO intervencao VALUES('avaria', 'por atribuir', 200, '2021-04-30','2021-05-01')
    INSERT INTO intervencao VALUES('avaria', 'concluido', 300, '2021-04-30', '2021-05-05')


COMMIT
