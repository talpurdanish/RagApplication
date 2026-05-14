# Stage 1: Build
FROM node:lts-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Stage 2: Production
FROM nginx:stable-alpine
RUN apk add --no-cache curl && \
    rm -rf /var/cache/apk/*
COPY --from=build /app/dist/RagClient/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
COPY nginx-default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:80 || exit 1
CMD ["nginx", "-g", "daemon off;"]
