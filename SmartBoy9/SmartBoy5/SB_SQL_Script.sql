use SmartBoyDatabase1;

select * from Track_SB
select * from Album_SB
select * from Artist_SB
select * from Track_Album_Reln
select * from Track_Artist_Reln
select * from ID_SB


delete from Track_SB where Title != 'XYZ'
delete from Artist_SB where Artist_Name != 'XYZ'
delete from Album_SB where Album_Name != 'XYZ'

delete from ID_SB where Hash = '233fa29fd3fee4ec9b7255d70c6fed77'

delete from Track_Album_Reln where id = '233AQA233DtF'


delete from Album_SB where Album_Status = 'Official' or Album_Status = 'Bootleg' or Album_Status = 'Promotion'

SELECT DISTINCT idr.Hash,t.Title,t.Counter,t.Track_length,al.Album_Name,ar.Artist_Name FROM Track_SB t JOIN Track_Album_Reln ta ON t.MB_TrackID = ta.MB_Track_ID JOIN Album_SB al ON ta.MB_AlbumID = al.MB_Release_ID JOIN Track_Artist_Reln tar ON t.MB_TrackID = tar.MB_Track_ID JOIN Artist_SB ar ON tar.MB_ArtistID = ar.MB_Artist_ID JOIN ID_SB idr ON idr.MB_Track_ID = t.MB_TrackID where al.Album_Name = 'Barfi!';



delete from Artist_SB where Artist_Type = 'Person' or Artist_Type = 'Group'
delete from Track_SB where Track_length != 100000
delete from Album_SB
