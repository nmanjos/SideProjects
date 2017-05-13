"""
Definition of urls for mytweets.
"""

from datetime import datetime
from django.conf.urls import url
import django.contrib.auth.views

import app.forms
import app.views

# Uncomment the next lines to enable the admin:
# from django.conf.urls import include
# from django.contrib import admin
# admin.autodiscover()


from django.conf.urls import patterns, include, url
from django.contrib import admin
from tweets import views
admin.autodiscover()
urlpatterns = patterns('',
url(r'^$', views.index, name='index'),
url(r'^admin/', include(admin.site.urls)),
)



urlpatterns = [
    # Examples:
    url(r'^$', views.index, name='index'),
    url(r'^admin/', include(admin.site.urls)),
    

    # Uncomment the admin/doc line below to enable admin documentation:
    # url(r'^admin/doc/', include('django.contrib.admindocs.urls')),

    # Uncomment the next line to enable the admin:
    # url(r'^admin/', include(admin.site.urls)),
]
