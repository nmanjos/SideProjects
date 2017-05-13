"""
Definition of views.
"""
from django.views.generic.edit import CreateView, UpdateView, DeleteView
from django.shortcuts import render, redirect
from django.contrib.auth import authenticate, login
from django.http import HttpRequest
from django.template import RequestContext,loader
from datetime import datetime
from django.http import HttpResponse
from .models import System,Faction,FHist,SHist
from django.views import generic
from django.core.urlresolvers import reverse_lazy
from django.views.generic import View
from .forms import UserForm


def index(request):
    all_systems = Systems
    
    context = {'all_systems': all_systems}
    return render(request,'app/index.html',context)

                        
def detail(request, id):
    try:
        system = System.objects.get(pk=id)
    except System.DoesNotExist:
        raise Http404("System does not exist")
    return render(request, 'app/detail.html', {'system': system})



def Systems(request):
    systems_list = Systems.objects.order_by('-id')[:5]   
    context = {'Systems_list': systems_list}
    return render(request, 'app/index.html', context)



def home(request):
    """Renders the home page."""
    assert isinstance(request, HttpRequest)
    return render(
        request,
        'app/index.html',
        {
            'title':'Home Page',
            'year':datetime.now().year,
        }
    )

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
            'message':'Your application description page.',
            'year':datetime.now().year,
        }
    )





# class UserFormView(View):   form_class = UserForm
#   template_name = 'app/registration_form.html'

 #   def get(self, request):
#        form = self.form_class(None)
 #       return render(request, self.template_name,{'form': form})

 #   def post(elf, request):
 #      form = self.form_class(request.Post)
 #
  #     if form.is_valid():
#
 #          user = form.save(commit=False)
 #          # Clean Normalized data
#           username = form.cleaned_data['username']
 #          password = form.cleaned_data['password']
 #          user.set_password()
#           user.save()
           
#           user = authenticate(username=username, password=password)
#
 #          if user is not None:
#               if user.is_active:
#                   login(request, user)

#        
  #     return render(request, self.template_name,{'form': form})






