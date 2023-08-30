FROM node:18-alpine as builder

ENV REACT_APP_IDENTITY=/identity/api
ENV REACT_APP_LOGIN=/identity
ENV REACT_APP_MAIN=/api
ENV REACT_APP_TOKEN_NAME=msjwt
ENV REACT_APP_BREAK_INACTIVITY=false

WORKDIR /app

COPY package.json .

RUN npm install

COPY . .

RUN npm run build

FROM nginx

RUN apt update \ 
  && apt install -y mc

COPY --from=builder /app/build /usr/share/nginx/html

COPY default.conf /etc/nginx/conf.d/