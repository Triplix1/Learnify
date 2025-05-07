FROM node:latest as angular
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install --force
COPY . .
RUN npm run build --configuration=production

FROM nginx:latest
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=angular /app/dist/client /usr/share/nginx/html