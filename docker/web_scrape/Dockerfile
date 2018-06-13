FROM ubuntu:16.04

# Install Python
RUN apt-get update \
  && apt-get install -y python3-pip python3-dev \
  && cd /usr/local/bin \
  && ln -s /usr/bin/python3 python \
  && pip3 install --upgrade pip
  

# Install Additional Tools
RUN apt-get update && apt-get install -y \
    curl apt-utils apt-transport-https debconf-utils gcc build-essential \
    && rm -rf /var/lib/apt/lists/*  
	

# install google chrome
RUN curl https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google-chrome.list'
RUN apt-get -y update
RUN apt-get install -y google-chrome-stable

# install chromedriver
RUN apt-get install -yqq unzip
RUN wget -O /tmp/chromedriver.zip http://chromedriver.storage.googleapis.com/`curl -sS chromedriver.storage.googleapis.com/LATEST_RELEASE`/chromedriver_linux64.zip
RUN unzip /tmp/chromedriver.zip chromedriver -d /usr/local/bin/

# install SQL Server Python SQL Server connector module - pyodbc
RUN pip install pyodbc==3.0.10
 
# set display port to avoid crash
ENV DISPLAY=:99

# install selenium
RUN pip install selenium==3.12.0

# install beautifulsoup
RUN pip install beautifulsoup4

# install requests package
RUN pip install requests

# install necessary locales
RUN apt-get update && apt-get install -y locales \
    && echo "en_US.UTF-8 UTF-8" > /etc/locale.gen \
    && locale-gen

# Set the working directory to /app
WORKDIR /app

# Copy the contents to the app folder
ADD . /app

# Run a_sample_scrape.py when the container launches
CMD ["python", "a_sample_scrape.py"]	