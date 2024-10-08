alter table ama_session
    drop column planned_end;

alter table comment
    add deleted_at date;

alter table comment
    add created_at date not null;

create table "like"
(
    user_id         uuid not null
        constraint user_id
            references "user",
    ama_question_id uuid not null
        constraint ama_question_id
            references ama_question,
    constraint like_id
        primary key (ama_question_id, user_id)
);

alter table review
    add title varchar(200) not null;

alter table work_on
    add details varchar(1000);

alter table entry
    add type varchar(50);