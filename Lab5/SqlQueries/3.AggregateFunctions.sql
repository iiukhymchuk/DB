-- Створити підсумковий запит за допомогою агрегативних функції SQL – SUM( ), Count ( ), AVG ( ), MIN ( ), MAX ( ).
USE AdventureWorks2017;

SELECT
	s.[Name] [State]
	,Count(s.[Name]) [Count]
FROM Person.Person p
JOIN Person.BusinessEntityAddress bea ON p.BusinessEntityID = bea.BusinessEntityID
JOIN Person.[Address] a ON bea.AddressID = a.AddressID
JOIN Person.AddressType t ON bea.AddressTypeID = t.AddressTypeID
JOIN Person.StateProvince s ON a.StateProvinceId = s.StateProvinceId
GROUP BY s.[Name]
HAVING Count(s.[Name]) > 100