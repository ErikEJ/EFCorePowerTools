@echo off
@echo This cmd file creates a Data API Builder configuration based on the chosen database objects.
@echo To run the cmd, create an .env file with the following contents:
@echo dab-connection-string=your connection string
@echo ** Make sure to exclude the .env file from source control **
@echo **
dotnet tool install -g Microsoft.DataApiBuilder
dab init -c dab-config.json --database-type mssql --connection-string "@env('dab-connection-string')" --host-mode Development
@echo Adding tables
dab add "Album" --source "[dbo].[Album]" --fields.include "AlbumId,Title,ArtistId" --permissions "anonymous:*" 
dab add "Artist" --source "[dbo].[Artist]" --fields.include "ArtistId,Name" --permissions "anonymous:*" 
dab add "Genre" --source "[dbo].[Genre]" --fields.include "GenreId,Name" --permissions "anonymous:*" 
dab add "MediaType" --source "[dbo].[MediaType]" --fields.include "MediaTypeId,Name" --permissions "anonymous:*" 
dab add "Playlist" --source "[dbo].[Playlist]" --fields.include "PlaylistId,Name" --permissions "anonymous:*" 
dab add "PlaylistTrack" --source "[dbo].[PlaylistTrack]" --fields.include "PlaylistId,TrackId" --permissions "anonymous:*" 
dab add "Track" --source "[dbo].[Track]" --fields.include "TrackId,Name,AlbumId,MediaTypeId,GenreId,Composer,Milliseconds,Bytes,UnitPrice" --permissions "anonymous:*" 
@echo Adding views and tables without primary key
@echo Adding relationships
dab update Album --relationship Artist --target.entity Artist --cardinality one
dab update Artist --relationship Album --target.entity Album --cardinality many
dab update PlaylistTrack --relationship Playlist --target.entity Playlist --cardinality one
dab update Playlist --relationship PlaylistTrack --target.entity PlaylistTrack --cardinality many
dab update PlaylistTrack --relationship Track --target.entity Track --cardinality one
dab update Track --relationship PlaylistTrack --target.entity PlaylistTrack --cardinality many
dab update Track --relationship Album --target.entity Album --cardinality one
dab update Album --relationship Track --target.entity Track --cardinality many
dab update Track --relationship Genre --target.entity Genre --cardinality one
dab update Genre --relationship Track --target.entity Track --cardinality many
dab update Track --relationship MediaType --target.entity MediaType --cardinality one
dab update MediaType --relationship Track --target.entity Track --cardinality many
@echo Adding stored procedures
@echo **
@echo ** run 'dab validate' to validate your configuration **
@echo ** run 'dab start' to start the development API host **
