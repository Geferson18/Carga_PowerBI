
select * from [dbo].[TMP_AGT_Historial_Minutes]
-- truncate table [dbo].[TMP_AGT_Historial_Minutes]

select count(1) from [dbo].[TMP_AGT_Cliente];
select count(1) from [dbo].[TMP_AGT_Minutes];
select count(1) from [dbo].[TMP_AGT_Builder];
select count(1) from [dbo].[TMP_AGT_Proyects];
-- 
 
truncate table [dbo].[TMP_AGT_Historial_Minutes];
truncate table [dbo].[TMP_AGT_Historial_Citas];
truncate table [dbo].[TMP_AGT_Agentes];
truncate table [dbo].[TMP_AGT_Typologies];
truncate table [dbo].[TMP_AGT_Cliente];
truncate table [dbo].[TMP_AGT_Minutes];
truncate table [dbo].[TMP_AGT_Builder];
truncate table [dbo].[TMP_AGT_Proyects];

