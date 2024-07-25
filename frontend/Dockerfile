FROM node:22-alpine AS build-stage

WORKDIR /usr/src/app

COPY . .

RUN npm install && npm run build

FROM node:22-alpine

WORKDIR /usr/src/app


COPY --from=build-stage /usr/src/app/dist /usr/src/app/dist

RUN npm install -g serve

RUN adduser -D fe_user

USER fe_user

EXPOSE 5173

CMD serve -s -l 5173 dist
