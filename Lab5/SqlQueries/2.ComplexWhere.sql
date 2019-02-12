-- Створити запит з використанням складної довільної умови.
USE AdventureWorks2017;

SELECT
	ISNULL(p.[Title] + ' ', '') + ISNULL(p.[FirstName] + ' ', '')
		+ ISNULL(p.[MiddleName] + ' ', '') + ISNULL(p.[LastName], '') FullName
	,t.[Name] AddressType
	,a.AddressLine1
	,a.City
	,s.[Name] [State]
FROM Person.Person p
JOIN Person.BusinessEntityAddress bea ON p.BusinessEntityID = bea.BusinessEntityID
JOIN Person.[Address] a ON bea.AddressID = a.AddressID
JOIN Person.AddressType t ON bea.AddressTypeID = t.AddressTypeID
JOIN Person.StateProvince s ON a.StateProvinceId = s.StateProvinceId
WHERE t.[Name] = 'Home'
AND (s.[Name] = 'California' OR s.[Name] = 'Washington')
AND a.City LIKE '[U-Z]%'