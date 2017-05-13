from django.views import generic
from .models import Faction,System,GovType
from django.http import HttpRequest
from django.shortcuts import render
from datetime import datetime

class IndexView(generic.ListView):
    template_name = 'app/index.html'


    def get_queryset(self):
        return GovType.objects.all()



class DetailView(generic.DetailView):
    model = GovType
    template_name = 'app/detail.html'
    

class Systemlist(generic.ListView):
    model = System
    template_name = "app/systems.html"
    paginate_by = 35
    def get_context_data(self, **kwargs):
        context = super(Systemlist, self).get_context_data(**kwargs)
        context['syscount'] = System.objects.count()
       # context['system_list'] = System.objects.all()
        return context



class SystemDetail(generic.DetailView):
    template_name = "app/system.html"
    model = System

# Default Menu Functions

def home(request):
    """Renders the home page."""
    assert isinstance(request, HttpRequest)
    context = { 
        'title':'Home Page',
        'year':datetime.now().year,
        }

    if request.user.is_authenticated() and request.user.is_staff:
        #print(GovType.objects.all())
        #for instance in GovType.objects.all():
        queryset = GovType.objects.all().order_by('type')
        context = {
        'title':'Home Page',
        'year':datetime.now().year,
        'body_title': 'Welcome to EDBGS Database',
        'queryset': queryset,
        }
    return render(request,'app/index.html',context)

def contact(request):
    """Renders the contact page."""
    assert isinstance(request, HttpRequest)
    return render(
        request,
        'app/contact.html',
        {
            'title':'Contact',
            'message':'Your contact page.',
            'year':datetime.now().year,
        }
    )

def about(request):
    """Renders the about page."""
    assert isinstance(request, HttpRequest)
    return render(
        request,
        'app/about.html',
        {
            'title':'About',
            'message':'Elite Dangerous BGS Database - A Diferent way to play ED !',
            'year':datetime.now().year,
        }
    )