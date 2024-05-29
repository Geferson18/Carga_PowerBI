INSERT INTO [dbo].[TMP_AGT_Typologies]([Id_typologies],[id],[mongo_propetyIds],[typology_id],[project_id],[project_Name],[Type],[stock],[min_type_currency],[max_type_currency],[min_price],[max_price],[min_bedrooms],[max_bedrooms],[min_meters],[max_meters],[position],[status_value],[mongo_TypologyStatusValue],[Typologia_Name],[CreatedAt],[UpdatedAT])
VALUES ('5f0bff314678460016d71cd3','[]','15766',898,'A-01',3,1,0,1,354787,354787,2,2,210,210,1,3,3,'A-01','2018-07-09','1980-01-01')

select * from  [dbo].[TMP_AGT_Typologies]
-- truncate table [dbo].[TMP_AGT_Typologies];
select * from  [dbo].[TMP_AGT_Cliente]
-- truncate table [dbo].[TMP_AGT_Cliente];


SELECt       anio, Paginas, rebote
-- sum([Sesiones]) as sesiones, sum( contactos) as contac,
-- sum([RU_P]) as cupp,  sum(RUM) as rum , sum(RU_U) as DU
      FROM [dbo].[Consolidado__mensual]
  where anio=2021 AND MES=1
 