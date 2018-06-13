'''

The below scripts expects that you have installed latest version of chrome and chrome driver. Make sure chromedriver executable is added to your OS's path.
Also make sure you have the necessary python libraries : selenium, beautifulsoup, requests

'''

import requests
import json
import time
from urllib.parse import unquote
from bs4 import BeautifulSoup
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
import sys, traceback
import tempfile

chrome_options = Options()
chrome_options.add_argument('--headless') #comment this line to let the script launch chrome.
chrome_options.add_argument('--no-sandbox')
chrome_options.add_argument('--disable-dev-shm-usage')
driver = webdriver.Chrome(chrome_options=chrome_options)


t0 = time.time()

'''
tl;dr license / usage

The below scrape of geeks for geeks is just to show case scraping abilities. I'm not sure what their policies are about scraping or using the data.
The below is for a purely academical purpose to show the use of selenium beautiful soup and other python libraries in the world of web scraping

'''

print ("Problems from geeks from geeks landing page")

# Crawl the page

url = "https://www.geeksforgeeks.org/"
driver.get(url)
driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")

# Create the Parser 

soup = BeautifulSoup(driver.page_source, 'html.parser')
ahrefs = soup.find_all('a', attrs={'rel' : 'bookmark'})
deep_dive_problem = ''

for ahref in ahrefs:
    try:
        print("{}".format(ahref['title']))
        deep_dive_problem = ahref['href']
    except Exception as e: 
        print("Unable to fetch the tile {}".format(ahref))

print ("Deep diving to last problem ")

driver.get(deep_dive_problem)
problem_soup = BeautifulSoup(driver.page_source, 'html.parser')
pblm_ahrefs = problem_soup.find_all('a')  

print("All links for the problem page ")

for ahref in pblm_ahrefs:
    try:
        print(ahref.text)
    except Exception as e: 
        print("unable to get text {}".format(ahref))      

t1 = time.time()
driver.close()

print("Total time to for the crawl is %d seconds." %(t1-t0))