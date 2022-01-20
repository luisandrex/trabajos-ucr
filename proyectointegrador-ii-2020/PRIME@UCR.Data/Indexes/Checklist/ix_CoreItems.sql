CREATE INDEX ix_CoreItems
ON item(IDSuperItem)
WHERE IDSuperItem IS NULL 
