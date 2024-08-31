-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2024-07-15 19:44:10.682

-- tables
-- Table: album
CREATE TABLE album (
    id uuid  NOT NULL,
    CONSTRAINT album_pk PRIMARY KEY (id)
);

-- Table: album_music_genre
CREATE TABLE album_music_genre (
    album_id uuid  NOT NULL,
    music_genre_id uuid  NOT NULL,
    CONSTRAINT album_music_genre_pk PRIMARY KEY (album_id,music_genre_id)
);

-- Table: ama_question
CREATE TABLE ama_question (
    id uuid  NOT NULL,
    ama_session_id uuid  NOT NULL,
    user_id uuid  NOT NULL,
    content varchar(1000)  NOT NULL,
    answer varchar(3000)  NULL,
    CONSTRAINT ama_question_pk PRIMARY KEY (id)
);

-- Table: ama_session
CREATE TABLE ama_session (
    id uuid  NOT NULL,
    author_id uuid  NOT NULL,
    start timestamp  NOT NULL,
    "end" timestamp  NOT NULL,
    planned_end timestamp  NULL,
    CONSTRAINT ama_session_pk PRIMARY KEY (id)
);

-- Table: answer
CREATE TABLE answer (
    id uuid  NOT NULL,
    is_correct boolean  NOT NULL,
    text varchar(20)  NOT NULL,
    question_id uuid  NOT NULL,
    CONSTRAINT answer_pk PRIMARY KEY (id)
);

-- Table: article
CREATE TABLE article (
    id uuid  NOT NULL,
    title varchar(200)  NOT NULL,
    content text  NOT NULL,
    user_id uuid  NOT NULL,
    timestamp timestamp  NOT NULL,
    CONSTRAINT article_pk PRIMARY KEY (id)
);

-- Table: author
CREATE TABLE author (
    id uuid  NOT NULL,
    name varchar(150)  NOT NULL,
    surname varchar(150)  NOT NULL,
    bio text  NOT NULL,
    user_id uuid  NULL,
    profile_picture_id uuid  NOT NULL,
    CONSTRAINT author_pk PRIMARY KEY (id)
);

-- Table: author_role
CREATE TABLE author_role (
    id uuid  NOT NULL,
    name varchar(100)  NOT NULL,
    CONSTRAINT author_role_pk PRIMARY KEY (id)
);

-- Table: book
CREATE TABLE book (
    id uuid  NOT NULL,
    isbn varchar(50)  NOT NULL,
    synopsis text  NOT NULL,
    CONSTRAINT book_pk PRIMARY KEY (id)
);

-- Table: book_book_genre
CREATE TABLE book_book_genre (
    book_id uuid  NOT NULL,
    book_genre_id uuid  NOT NULL,
    CONSTRAINT book_book_genre_pk PRIMARY KEY (book_id,book_genre_id)
);

-- Table: book_genre
CREATE TABLE book_genre (
    id uuid  NOT NULL,
    name varchar(200)  NOT NULL,
    CONSTRAINT book_genre_pk PRIMARY KEY (id)
);

-- Table: cinematic_genre
CREATE TABLE cinematic_genre (
    id uuid  NOT NULL,
    name varchar(200)  NOT NULL,
    CONSTRAINT cinematic_genre_pk PRIMARY KEY (id)
);

-- Table: comment
CREATE TABLE comment (
    id uuid  NOT NULL,
    entry_id uuid  NOT NULL,
    parent_comment_id uuid  NULL,
    user_id uuid  NOT NULL,
    content varchar(1000)  NOT NULL,
    CONSTRAINT id PRIMARY KEY (id)
);

-- Table: cover_photo
CREATE TABLE cover_photo (
    id uuid  NOT NULL,
    photo bytea  NOT NULL,
    CONSTRAINT cover_photo_pk PRIMARY KEY (id)
);

-- Table: developer
CREATE TABLE developer (
    id uuid  NOT NULL,
    name varchar(255)  NOT NULL,
    CONSTRAINT developer_pk PRIMARY KEY (id)
);

-- Table: entry
CREATE TABLE entry (
    id uuid  NOT NULL,
    name varchar(150)  NOT NULL,
    description text  NOT NULL,
    release date  NOT NULL,
    cover_photo_id uuid  NOT NULL,
    CONSTRAINT entry_pk PRIMARY KEY (id)
);

-- Table: episode
CREATE TABLE episode (
    id uuid  NOT NULL,
    Series_id uuid  NOT NULL,
    synopsis text  NOT NULL,
    CONSTRAINT episode_pk PRIMARY KEY (id)
);

-- Table: friendship
CREATE TABLE friendship (
    user_id uuid  NOT NULL,
    user_2_id uuid  NOT NULL,
    approved boolean  NOT NULL,
    CONSTRAINT friendship_pk PRIMARY KEY (user_id,user_2_id)
);

-- Table: game
CREATE TABLE game (
    id uuid  NOT NULL,
    synopsis text  NOT NULL,
    CONSTRAINT game_pk PRIMARY KEY (id)
);

-- Table: game_developer
CREATE TABLE game_developer (
    game_id uuid  NOT NULL,
    developer_id uuid  NOT NULL,
    CONSTRAINT game_developer_pk PRIMARY KEY (game_id,developer_id)
);

-- Table: game_game_genre
CREATE TABLE game_game_genre (
    game_id uuid  NOT NULL,
    game_genre_id uuid  NOT NULL,
    CONSTRAINT game_game_genre_pk PRIMARY KEY (game_id,game_genre_id)
);

-- Table: game_genre
CREATE TABLE game_genre (
    id uuid  NOT NULL,
    name varchar(200)  NOT NULL,
    CONSTRAINT game_genre_pk PRIMARY KEY (id)
);

-- Table: movie
CREATE TABLE movie (
    id uuid  NOT NULL,
    synopsis text  NOT NULL,
    CONSTRAINT movie_pk PRIMARY KEY (id)
);

-- Table: movie_cinematic_genre
CREATE TABLE movie_cinematic_genre (
    movie_id uuid  NOT NULL,
    cinematic_genre_id uuid  NOT NULL,
    CONSTRAINT movie_cinematic_genre_pk PRIMARY KEY (movie_id,cinematic_genre_id)
);

-- Table: music_genre
CREATE TABLE music_genre (
    id uuid  NOT NULL,
    name varchar(200)  NOT NULL,
    CONSTRAINT music_genre_pk PRIMARY KEY (id)
);

-- Table: profile_picture
CREATE TABLE profile_picture (
    id uuid  NOT NULL,
    picture bytea  NOT NULL,
    CONSTRAINT profile_picture_pk PRIMARY KEY (id)
);

-- Table: quiz
CREATE TABLE quiz (
    entry_id uuid  NOT NULL,
    id uuid  NOT NULL,
    created_at timestamp  NOT NULL,
    CONSTRAINT quiz_pk PRIMARY KEY (id)
);

-- Table: quiz_question
CREATE TABLE quiz_question (
    id uuid  NOT NULL,
    quiz_id uuid  NOT NULL,
    text varchar(200)  NOT NULL,
    added_at timestamp  NOT NULL,
    CONSTRAINT quiz_question_pk PRIMARY KEY (id)
);

-- Table: quiz_taking
CREATE TABLE quiz_taking (
    user_id uuid  NOT NULL,
    quiz_id uuid  NOT NULL,
    score int  NOT NULL,
    id uuid  NOT NULL,
    takenAt timestamp  NOT NULL,
    CONSTRAINT quiz_taking_pk PRIMARY KEY (id)
);

-- Table: quiz_writing
CREATE TABLE quiz_writing (
    user_id uuid  NOT NULL,
    quiz_id uuid  NOT NULL,
    CONSTRAINT quiz_writing_pk PRIMARY KEY (user_id,quiz_id)
);

-- Table: rating
CREATE TABLE rating (
    id uuid  NOT NULL,
    grade int  NOT NULL,
    user_id uuid  NOT NULL,
    Entry_id uuid  NOT NULL,
    modifiedAt timestamp  NOT NULL,
    CONSTRAINT rating_pk PRIMARY KEY (id)
);

-- Table: review
CREATE TABLE review (
    user_id uuid  NOT NULL,
    entry_id uuid  NOT NULL,
    content varchar(2000)  NOT NULL,
    CONSTRAINT review_pk PRIMARY KEY (user_id,entry_id)
);

-- Table: role
CREATE TABLE role (
    Id uuid  NOT NULL,
    name varchar(20)  NOT NULL,
    CONSTRAINT role_pk PRIMARY KEY (Id)
);

-- Table: role_user
CREATE TABLE role_user (
    Role_Id uuid  NOT NULL,
    User_id uuid  NOT NULL,
    CONSTRAINT role_user_pk PRIMARY KEY (Role_Id,User_id)
);

-- Table: series
CREATE TABLE series (
    id uuid  NOT NULL,
    CONSTRAINT series_pk PRIMARY KEY (id)
);

-- Table: series_cinematic_genre
CREATE TABLE series_cinematic_genre (
    series_id uuid  NOT NULL,
    cinematic_genre_id uuid  NOT NULL,
    CONSTRAINT series_cinematic_genre_pk PRIMARY KEY (series_id,cinematic_genre_id)
);

-- Table: song
CREATE TABLE song (
    id uuid  NOT NULL,
    lyrics text  NULL,
    CONSTRAINT song_pk PRIMARY KEY (id)
);

-- Table: song_album
CREATE TABLE song_album (
    song_id uuid  NOT NULL,
    album_id uuid  NOT NULL,
    CONSTRAINT song_album_pk PRIMARY KEY (song_id,album_id)
);

-- Table: song_music_genre
CREATE TABLE song_music_genre (
    song_id uuid  NOT NULL,
    music_genre_id uuid  NOT NULL,
    CONSTRAINT song_music_genre_pk PRIMARY KEY (song_id,music_genre_id)
);

-- Table: user
CREATE TABLE "user" (
    id uuid  NOT NULL,
    username varchar(100)  NOT NULL,
    email varchar(150)  NOT NULL,
    password_hash varchar(200)  NOT NULL,
    profile_picture_id uuid  NOT NULL,
    CONSTRAINT user_pk PRIMARY KEY (id)
);

-- Table: vote
CREATE TABLE vote (
    user_id uuid  NOT NULL,
    comment_id uuid  NOT NULL,
    is_positive boolean  NOT NULL,
    CONSTRAINT vote_pk PRIMARY KEY (user_id,comment_id)
);

-- Table: work_on
CREATE TABLE work_on (
    author_id uuid  NOT NULL,
    entry_id uuid  NOT NULL,
    author_role_id uuid  NOT NULL,
    id uuid  NOT NULL,
    CONSTRAINT work_on_pk PRIMARY KEY (id)
);

-- foreign keys
-- Reference: Album_Entry (table: album)
ALTER TABLE album ADD CONSTRAINT Album_Entry
    FOREIGN KEY (id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Book_Entry (table: book)
ALTER TABLE book ADD CONSTRAINT Book_Entry
    FOREIGN KEY (id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Episode_Entry (table: episode)
ALTER TABLE episode ADD CONSTRAINT Episode_Entry
    FOREIGN KEY (id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Episode_Series (table: episode)
ALTER TABLE episode ADD CONSTRAINT Episode_Series
    FOREIGN KEY (Series_id)
    REFERENCES series (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Game_Entry (table: game)
ALTER TABLE game ADD CONSTRAINT Game_Entry
    FOREIGN KEY (id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Movie_Entry (table: movie)
ALTER TABLE movie ADD CONSTRAINT Movie_Entry
    FOREIGN KEY (id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Question_Quiz (table: quiz_question)
ALTER TABLE quiz_question ADD CONSTRAINT Question_Quiz
    FOREIGN KEY (quiz_id)
    REFERENCES quiz (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Quiz_entry (table: quiz)
ALTER TABLE quiz ADD CONSTRAINT Quiz_entry
    FOREIGN KEY (entry_id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Role_User_Role (table: role_user)
ALTER TABLE role_user ADD CONSTRAINT Role_User_Role
    FOREIGN KEY (Role_Id)
    REFERENCES role (Id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Role_User_User (table: role_user)
ALTER TABLE role_user ADD CONSTRAINT Role_User_User
    FOREIGN KEY (User_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Series_Entry (table: series)
ALTER TABLE series ADD CONSTRAINT Series_Entry
    FOREIGN KEY (id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: Song_Entry (table: song)
ALTER TABLE song ADD CONSTRAINT Song_Entry
    FOREIGN KEY (id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: album_music_genre_album (table: album_music_genre)
ALTER TABLE album_music_genre ADD CONSTRAINT album_music_genre_album
    FOREIGN KEY (album_id)
    REFERENCES album (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: album_music_genre_music_genre (table: album_music_genre)
ALTER TABLE album_music_genre ADD CONSTRAINT album_music_genre_music_genre
    FOREIGN KEY (music_genre_id)
    REFERENCES music_genre (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: ama_session_author (table: ama_session)
ALTER TABLE ama_session ADD CONSTRAINT ama_session_author
    FOREIGN KEY (author_id)
    REFERENCES author (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: answer_question (table: answer)
ALTER TABLE answer ADD CONSTRAINT answer_question
    FOREIGN KEY (question_id)
    REFERENCES quiz_question (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: article_user (table: article)
ALTER TABLE article ADD CONSTRAINT article_user
    FOREIGN KEY (user_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: author_profile_picture (table: author)
ALTER TABLE author ADD CONSTRAINT author_profile_picture
    FOREIGN KEY (profile_picture_id)
    REFERENCES profile_picture (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: author_user (table: author)
ALTER TABLE author ADD CONSTRAINT author_user
    FOREIGN KEY (user_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: book_book_genre_book (table: book_book_genre)
ALTER TABLE book_book_genre ADD CONSTRAINT book_book_genre_book
    FOREIGN KEY (book_id)
    REFERENCES book (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: book_book_genre_book_genre (table: book_book_genre)
ALTER TABLE book_book_genre ADD CONSTRAINT book_book_genre_book_genre
    FOREIGN KEY (book_genre_id)
    REFERENCES book_genre (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: comment_comment (table: comment)
ALTER TABLE comment ADD CONSTRAINT comment_comment
    FOREIGN KEY (parent_comment_id)
    REFERENCES comment (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: comment_entry (table: comment)
ALTER TABLE comment ADD CONSTRAINT comment_entry
    FOREIGN KEY (entry_id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: comment_user (table: comment)
ALTER TABLE comment ADD CONSTRAINT comment_user
    FOREIGN KEY (user_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: entry_cover_photo (table: entry)
ALTER TABLE entry ADD CONSTRAINT entry_cover_photo
    FOREIGN KEY (cover_photo_id)
    REFERENCES cover_photo (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: friendship_user (table: friendship)
ALTER TABLE friendship ADD CONSTRAINT friendship_user
    FOREIGN KEY (user_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: friendship_user2 (table: friendship)
ALTER TABLE friendship ADD CONSTRAINT friendship_user2
    FOREIGN KEY (user_2_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: game_developer_developer (table: game_developer)
ALTER TABLE game_developer ADD CONSTRAINT game_developer_developer
    FOREIGN KEY (developer_id)
    REFERENCES developer (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: game_developer_game (table: game_developer)
ALTER TABLE game_developer ADD CONSTRAINT game_developer_game
    FOREIGN KEY (game_id)
    REFERENCES game (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: game_game_genre_game (table: game_game_genre)
ALTER TABLE game_game_genre ADD CONSTRAINT game_game_genre_game
    FOREIGN KEY (game_id)
    REFERENCES game (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: game_game_genre_game_genre (table: game_game_genre)
ALTER TABLE game_game_genre ADD CONSTRAINT game_game_genre_game_genre
    FOREIGN KEY (game_genre_id)
    REFERENCES game_genre (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: movie_cinematic_genre_cinematic_genre (table: movie_cinematic_genre)
ALTER TABLE movie_cinematic_genre ADD CONSTRAINT movie_cinematic_genre_cinematic_genre
    FOREIGN KEY (cinematic_genre_id)
    REFERENCES cinematic_genre (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: movie_cinematic_genre_movie (table: movie_cinematic_genre)
ALTER TABLE movie_cinematic_genre ADD CONSTRAINT movie_cinematic_genre_movie
    FOREIGN KEY (movie_id)
    REFERENCES movie (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: question_ama_session (table: ama_question)
ALTER TABLE ama_question ADD CONSTRAINT question_ama_session
    FOREIGN KEY (ama_session_id)
    REFERENCES ama_session (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: question_user (table: ama_question)
ALTER TABLE ama_question ADD CONSTRAINT question_user
    FOREIGN KEY (user_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: quiz_taking_quiz (table: quiz_taking)
ALTER TABLE quiz_taking ADD CONSTRAINT quiz_taking_quiz
    FOREIGN KEY (quiz_id)
    REFERENCES quiz (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: quiz_taking_user (table: quiz_taking)
ALTER TABLE quiz_taking ADD CONSTRAINT quiz_taking_user
    FOREIGN KEY (user_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: quiz_writing_quiz (table: quiz_writing)
ALTER TABLE quiz_writing ADD CONSTRAINT quiz_writing_quiz
    FOREIGN KEY (quiz_id)
    REFERENCES quiz (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: quiz_writing_user (table: quiz_writing)
ALTER TABLE quiz_writing ADD CONSTRAINT quiz_writing_user
    FOREIGN KEY (user_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: rating_Entry (table: rating)
ALTER TABLE rating ADD CONSTRAINT rating_Entry
    FOREIGN KEY (Entry_id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: rating_user (table: rating)
ALTER TABLE rating ADD CONSTRAINT rating_user
    FOREIGN KEY (user_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: review_entry (table: review)
ALTER TABLE review ADD CONSTRAINT review_entry
    FOREIGN KEY (entry_id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: review_user (table: review)
ALTER TABLE review ADD CONSTRAINT review_user
    FOREIGN KEY (user_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: series_cinematic_genre_cinematic_genre (table: series_cinematic_genre)
ALTER TABLE series_cinematic_genre ADD CONSTRAINT series_cinematic_genre_cinematic_genre
    FOREIGN KEY (cinematic_genre_id)
    REFERENCES cinematic_genre (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: series_cinematic_genre_series (table: series_cinematic_genre)
ALTER TABLE series_cinematic_genre ADD CONSTRAINT series_cinematic_genre_series
    FOREIGN KEY (series_id)
    REFERENCES series (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: song_album_album (table: song_album)
ALTER TABLE song_album ADD CONSTRAINT song_album_album
    FOREIGN KEY (album_id)
    REFERENCES album (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: song_album_song (table: song_album)
ALTER TABLE song_album ADD CONSTRAINT song_album_song
    FOREIGN KEY (song_id)
    REFERENCES song (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: song_music_genre_music_genre (table: song_music_genre)
ALTER TABLE song_music_genre ADD CONSTRAINT song_music_genre_music_genre
    FOREIGN KEY (music_genre_id)
    REFERENCES music_genre (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: song_music_genre_song (table: song_music_genre)
ALTER TABLE song_music_genre ADD CONSTRAINT song_music_genre_song
    FOREIGN KEY (song_id)
    REFERENCES song (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: user_profile_picture (table: user)
ALTER TABLE "user" ADD CONSTRAINT user_profile_picture
    FOREIGN KEY (profile_picture_id)
    REFERENCES profile_picture (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: vote_comment (table: vote)
ALTER TABLE vote ADD CONSTRAINT vote_comment
    FOREIGN KEY (comment_id)
    REFERENCES comment (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: vote_user (table: vote)
ALTER TABLE vote ADD CONSTRAINT vote_user
    FOREIGN KEY (user_id)
    REFERENCES "user" (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: work_on_author (table: work_on)
ALTER TABLE work_on ADD CONSTRAINT work_on_author
    FOREIGN KEY (author_id)
    REFERENCES author (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: work_on_author_role (table: work_on)
ALTER TABLE work_on ADD CONSTRAINT work_on_author_role
    FOREIGN KEY (author_role_id)
    REFERENCES author_role (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- Reference: work_on_entry (table: work_on)
ALTER TABLE work_on ADD CONSTRAINT work_on_entry
    FOREIGN KEY (entry_id)
    REFERENCES entry (id)  
    NOT DEFERRABLE 
    INITIALLY IMMEDIATE
;

-- End of file.



-- EXAMPLE DATA

INSERT INTO role
VALUES ((SELECT uuid_in('e345fcaf-a7a0-4663-bdc3-6b498f598693')), 'User');

INSERT INTO role
VALUES ((SELECT uuid_in('aa2da3d4-6710-4714-9bc8-054023063118')), 'Admin');


INSERT INTO profile_picture
VALUES ((SELECT uuid_in('dbc2478f-ee4f-492e-afdd-7584a81e2baa')),
        PG_READ_BINARY_FILE('/data/tom.jpg'));


-- inserting an entry
INSERT INTO cover_photo
VALUES ((SELECT uuid_in('02ec0136-8beb-4481-9004-dd6fd2ad4072')),
        PG_READ_BINARY_FILE('/data/typewriter.jpg'));

INSERT INTO entry
VALUES ((SELECT uuid_in('2cd11d9b-51f4-4f0b-96a4-7e65881438ec')),
        'The catcher in the rye',
        'Lorem ipsum dolor sit amet',
        '05-27-1951',
        (SELECT uuid_in('02ec0136-8beb-4481-9004-dd6fd2ad4072')));

INSERT INTO book_genre
VALUES ((SELECT uuid_in('6fa89aba-d05d-4507-bdb3-b29c88b4c133')), 'Classics');
INSERT INTO book_genre
VALUES ((SELECT uuid_in('5ae2a462-d28c-413b-9a4a-a5937e4cbaaf')), 'Fiction');

INSERT INTO book
VALUES ((SELECT uuid_in('2cd11d9b-51f4-4f0b-96a4-7e65881438ec')), '978-9-9162-1269-1', 'Lorem ipsum dolor sit amet');

INSERT INTO book_book_genre
VALUES ((SELECT uuid_in('2cd11d9b-51f4-4f0b-96a4-7e65881438ec')),
        (SELECT uuid_in('6fa89aba-d05d-4507-bdb3-b29c88b4c133')));
INSERT INTO book_book_genre
VALUES ((SELECT uuid_in('2cd11d9b-51f4-4f0b-96a4-7e65881438ec')),
        (SELECT uuid_in('5ae2a462-d28c-413b-9a4a-a5937e4cbaaf')));


INSERT INTO entry
VALUES ((SELECT uuid_in('d13d7237-0e1a-402e-abec-e34bf4945d34')),
        'Another book',
        'Lorem ipsum dolor sit amet',
        '05-27-1961',
        (SELECT uuid_in('02ec0136-8beb-4481-9004-dd6fd2ad4072')));

INSERT INTO book
VALUES ((SELECT uuid_in('d13d7237-0e1a-402e-abec-e34bf4945d34')), '978-9-9162-1269-1', 'Lorem ipsum dolor sit amet');


INSERT INTO author
VALUES ((SELECT uuid_in('5f49e424-f74e-49c9-8d82-67a4202354df')), 'Jerome David', 'Salinger',
        'A newyorker notorious for his reclusive nature', NULL,
        (SELECT uuid_in('dbc2478f-ee4f-492e-afdd-7584a81e2baa')));

INSERT INTO author
VALUES ((SELECT uuid_in('7cfcec2b-0d68-4e91-857a-1016e8f50bb2')), 'Jed', 'Bartlet',
        'Former president of the United states and a recipient of the economics Nobel prize', NULL,
        (SELECT uuid_in('dbc2478f-ee4f-492e-afdd-7584a81e2baa')));



