USE AP2
GO;


BEGIN TRAN
	SET NOCOUNT ON

    INSERT INTO tipo_activo VALUES(111, 'piscina coisa')
    INSERT INTO tipo_activo VALUES(222, 'coisa para piscina')

    INSERT INTO activo VALUES('piscina',GETDATE(),1, 'Apple', '7775xx', 111)
	INSERT INTO activo VALUES('bomba',GETDATE(),1, 'Jacksonville', '8888xy', 222)
    INSERT INTO activo VALUES('bomba',GETDATE(),1, 'Jacksonville', '8888xy', 222)
    INSERT INTO activo VALUES('bomba',GETDATE(),1, 'Jacksonville', '8888xy', 222)



    INSERT INTO activo_filho VALUES(1, 2)
    INSERT INTO activo_filho VALUES(1, 3)
    INSERT INTO activo_filho VALUES(1, 4)


    INSERT INTO funcionario VALUES(2345, 'Mccoolville', '1997-04-22', 'rua sao pedro', 'oficial das piscinas', 928181727, 'mccool@hotmail.com')
    INSERT INTO funcionario VALUES(4444, 'Alice n Chainz', '1978-05-31', 'rua sá', 'reparador piscinas', 987878787, 'anc@hotmail.com')

    INSERT INTO activo_gerente VALUES(1, 1)
    INSERT INTO activo_gerente VALUES(2, 2)



COMMIT
