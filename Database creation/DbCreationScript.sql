use sqlscriptforum

DROP TABLE Comments;
DROP TABLE Posts;
DROP TABLE Subcategories;
DROP TABLE Categories;
DROP TABLE Users_Titles;
DROP TABLE Titles;
DROP TABLE Users;

CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(16),
    Email VARCHAR(100),
    Password VARCHAR(100),
    SelectedTitle VARCHAR(20),
    IsActive BIT,
    IsBanned BIT,
    Bio VARCHAR(250),
    Location VARCHAR(20),
    LastLoggedIn DATE,
    Role INT DEFAULT 0,
    CreatedAt DATE
)

CREATE TABLE Titles
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TitleName VARCHAR(20)
)

CREATE TABLE Users_Titles
(
    UserId INT REFERENCES Users(Id) ON DELETE CASCADE,
    TitleId INT REFERENCES Titles(Id) ON DELETE CASCADE,
    CONSTRAINT PK_UserTitle PRIMARY KEY(UserId, TitleId)
)

CREATE TABLE Categories
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(20)
)

CREATE TABLE Subcategories
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50),
    CategoryId INT REFERENCES Categories(Id) ON DELETE CASCADE
)

CREATE TABLE Posts
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(50),
    Content VARCHAR(500),
    NumberOfViews INT DEFAULT 0,
    NumberOfLikes INT DEFAULT 0,
    CreatedAt DATE,
    IsEdited BIT,
    LastEditedAt DATE,
    IsDeleted BIT,
    IsRemovedByAdmin BIT,
    UserId INT REFERENCES Users(Id) ON DELETE CASCADE,
    SubcategoryId INT REFERENCES Subcategories(Id) ON DELETE CASCADE
)

CREATE TABLE Comments
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Content VARCHAR(250),
    NumberOfLikes INT DEFAULT 0,
    CreatedAt DATE,
    IsEdited BIT,
    LastEditedAt DATE,
    IsDeleted BIT,
    IsRemovedByAdmin BIT,
    UserId INT REFERENCES Users(Id) ON DELETE NO ACTION, 
    PostId INT REFERENCES Posts(Id) ON DELETE CASCADE
)
