from django.conf.urls import url

from . import views

app_name = 'Elite Dangerous BGS Database'
urlpatterns = [
    url(r'^$', views.Systemlist.as_view(), name='systems'),
    url(r'^(?P<pk>[0-9]+)/$', views.SystemDetail.as_view(), name='system-detail'),
 #   url(r'^(?P<id>[0-9]+)/results/$', views.results, name='results'),
 #   url(r'^(?P<id>[0-9]+)/vote/$', views.vote, name='vote'),
 #   url(r'^register/$', views.UserFormView, name='register'),
]