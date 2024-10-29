
alter table ama_question
    add created_at timestamp not null;

alter table "user"
    alter column profile_picture_id drop not null;

alter table ama_session
    add title VARCHAR(100) not null;

alter table ama_session
    add description VARCHAR(1000) not null ;


alter table comment
alter column created_at type timestamp using created_at::timestamp;
