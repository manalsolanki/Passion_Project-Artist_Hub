# Passion_Project-Artist_Hub 

## Description
This platform is built for artist, here artist does not only include professional artists but anybody who excels or is passionate about any of the art forms can enroll here. So this way, an event organising company can direct search a particular artist from here and it can be benefical both the organiser as well as the artist.

## Entities
- Artist - Details of artist and in which field they are good at.(Many to Many with ArtForms)
- ArtForms - Lists all the artforms and in which discipline they belong to.( 1-M with Discipline & M-M with Artis)
- Discipline - Information about all the discipline and a short description.(1-M with ArtForm)

## Tasks

- [x] Create 3 tables to manage data (Using migrations)
- [x] Establish Relationships between them (1-M and M-M)
- [x] CRUD for all the three entitites
- [x] An artist can select more then one artform functionality.
- [x] Selecting an discipline from dropdown while creating a new artform
- [x] Profile Picture upload for Artist
- [x] Responsive Design
- [x] PArtially Implemented Authorization  

## Future Enchacements 

- Search Bar in the artist list where one can search by name or artforms.
- When artist created at that time he can upload image as well as select the art form he/she is interested in.
- Improve the Design of the website.

## Known Issues: 

- When we do a curl request for updating,creating or deleteing its not been authorize.
