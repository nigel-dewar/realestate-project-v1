

FROM node:11.13.0-alpine
# FROM node:11.13.0

# create destination directory
RUN mkdir -p /usr/src/nuxt-app
WORKDIR /usr/src/nuxt-app

# update and install dependency
RUN apk update && apk upgrade
RUN apk add git
# RUN apk add --no-cache bash

# copy the app, note .dockerignore
COPY . /usr/src/nuxt-app/
RUN npm install

# build necessary, even if no static files are needed,
# since it builds the server as well
ARG BASE_URL
ARG API_URL

ENV BASE_URL=$BASE_URL
ENV API_URL=$API_URL

RUN npm run build

# expose 5000 on container
EXPOSE 3000

# set app serving to permissive / assigned
ENV NUXT_HOST=0.0.0.0
# set app port
ENV NUXT_PORT=3000


# start the app
CMD [ "npm", "start" ]