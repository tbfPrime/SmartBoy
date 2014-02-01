use SmartBoyDatabase1;

select * from Track_SB
select * from Album_SB
select * from Artist_SB
select * from Track_Album_Reln
select * from Track_Artist_Reln
select * from ID_SB

delete from Track_SB where Track_length > 100000
delete from Album_SB where Album_Status = 'Official'

delete from Album_SB