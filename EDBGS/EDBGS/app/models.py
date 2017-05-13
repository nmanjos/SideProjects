"""
Definition of models.
"""

from datetime import date
from django.db import models


# Create your models here.


class GovType(models.Model):
    id = models.IntegerField(primary_key = True)
    game_id = models.CharField(max_length=35)
    type = models.CharField("Type of Government", max_length=35)
    description = models.TextField(blank=True, null=True)
    def __str__(self):
        return self.type

class SuperPower(models.Model):
    id = models.IntegerField(primary_key = True)
    game_id = models.CharField(max_length=35)
    name = models.CharField("Name of the Super Power", max_length=35)
    description = models.TextField(blank=True, null=True)
    def __str__(self):
        return self.name

class State(models.Model):
    id = models.IntegerField(primary_key = True)
    game_id = models.CharField(max_length=35)
    name = models.CharField("Type of state", max_length=35)
    description = models.TextField(blank=True, null=True)
    class Meta:
        abstract = True

    def __str__(self):
        return self.name


class Security(models.Model):
    id = models.IntegerField(primary_key = True)
    game_id = models.CharField(max_length=35)
    name = models.CharField("Type of Security", max_length=35)
    def __str__(self):
        return self.name

class Economy(models.Model):
    id = models.IntegerField(primary_key = True)
    game_id = models.CharField(max_length=35)
    name = models.CharField("Type of Economy", max_length=35)
    def __str__(self):
        return self.name

class Power(models.Model):
    id = models.IntegerField(primary_key = True)
    game_id = models.CharField(max_length=35)
    name = models.CharField("Power Name", max_length=35)
    def __str__(self):
        return self.name

class PowerState(models.Model):
    id = models.IntegerField(primary_key = True)
    game_id = models.CharField(max_length=35)
    name = models.CharField("System Power State", max_length=35)
    def __str__(self):
        return self.name

class Reserve(models.Model):
    id = models.IntegerField(primary_key = True)
    game_id = models.CharField(max_length=35)
    name = models.CharField("Type of state", max_length=35)

    def __str__(self):
        return self.name
     

class sState(State):
    pass
class fState(State):
    pass




class System(models.Model):
    id = models.IntegerField(primary_key = True)
    name = models.CharField("Name of the_System", max_length=35)
    govtype = models.ForeignKey(GovType, on_delete=models.PROTECT, null=True)
    population = models.BigIntegerField(blank=False, null=True)
    is_populated = models.BooleanField(default=False)
    updated_at = models.DateTimeField(blank=False, null=True)
    state = models.ForeignKey(sState, on_delete=models.PROTECT, null=True)
    security= models.ForeignKey(Security, on_delete=models.PROTECT, null=True)
    primary_economy = models.ForeignKey(Economy, on_delete=models.PROTECT, null=True)
    power = models.ForeignKey(Power, on_delete=models.PROTECT, null=True)
    power_state = models.ForeignKey(PowerState, on_delete=models.PROTECT, null=True)
    needs_permit = models.BooleanField(default=False)
    reserves  = models.ForeignKey(Reserve, on_delete=models.PROTECT, null=True)
    allegiance = models.ForeignKey(SuperPower, on_delete=models.PROTECT, null=True)

    def __str__(self):
        return self.name

   

class Faction(models.Model):
    id = models.IntegerField(primary_key=True)
    name = models.CharField(max_length=100)
    updated_at = models.DateTimeField(null=True)
    government= models.ForeignKey(GovType, on_delete=models.PROTECT, null=True)
    allegiance = models.ForeignKey(SuperPower, on_delete=models.PROTECT, null=True)
    state = models.ForeignKey(fState, on_delete=models.PROTECT, null=True)
    home_system = models.ForeignKey(System, on_delete=models.PROTECT, null=True)
    is_player_faction = models.BooleanField(default=False)

    def __str__(self):
        return self.name

class SHist(models.Model):
    class Meta:
        unique_together = ('Datecycle', 'shSystem')
        index_together = ["Datecycle", "shSystem"]
    
    Datecycle=models.DateField("Date", default=date.today)
    shSystem = models.ForeignKey(System)
    shFaction = models.ForeignKey(Faction)
    shGovtype = models.ForeignKey(GovType, on_delete=models.PROTECT, null=True)
    shState = models.ForeignKey(sState, on_delete=models.PROTECT, null=True)
    shSecurity= models.ForeignKey(Security, on_delete=models.PROTECT, null=True)
    shEconomy = models.ForeignKey(Economy, on_delete=models.PROTECT, null=True)
    shPower = models.ForeignKey(Power, on_delete=models.PROTECT, null=True)
    shPower_state = models.ForeignKey(PowerState, on_delete=models.PROTECT, null=True)
    shAllegiance = models.ForeignKey(SuperPower, on_delete=models.PROTECT, null=True)

class FHist(models.Model):
    class Meta:
        unique_together = ('Datecycle', 'fhSystem', 'fhFaction')
        index_together = ["Datecycle", "fhSystem", "fhFaction"]
    
    Datecycle=models.DateField("Date", default=date.today)
    fhSystem = models.ForeignKey(System)
    fhFaction = models.ForeignKey(Faction)
    fhState = models.ForeignKey(fState, on_delete=models.PROTECT, null=True)
    fhInfluence = models.FloatField("Influence", null=True)
    fhAllegiance = models.ForeignKey(SuperPower, on_delete=models.PROTECT, null=True)
    fhGovtype = models.ForeignKey(GovType, on_delete=models.PROTECT, null=True)


   # "message": {"StarSystem": "Pleiades Sector IH-V c2-5", "SystemFaction": "Cooper Research Associates", "timestamp": "2017-05-01T22:13:33Z", "SystemSecurity": "$SYSTEM_SECURITY_medium;",
   #  "SystemAllegiance": "Alliance", "SystemEconomy": "$economy_Military;", "StarPos": [-78.719, -138.031, -321.781], 
   #  "Factions": [
   #  {"Allegiance": "Federation", "FactionState": "None", "Influence": 0.18, "Name": "Pleiades Resource Enterprise", "Government": "Corporate"},
   #  {"Allegiance": "Alliance", "FactionState": "None", "Influence": 0.809, "Name": "Cooper Research Associates", "Government": "Democracy"}, 
   #  {"Allegiance": "Independent", "FactionState": "None", "Influence": 0.011, "Name": "Ursa Mercenaries", "Government": "Anarchy"}], 
   # "SystemGovernment": "$government_Democracy;"}}

   # "message": {"StarSystem": "Lepchaimyu", "SystemFaction": "Party of Yoru", "timestamp": "2017-05-01T22:13:31Z", "SystemSecurity": "$SYSTEM_SECURITY_low;", 
   # "SystemAllegiance": "Independent", "PowerplayState": "Exploited", "SystemEconomy": "$economy_Extraction;", "StarPos": [97.531, -79.313, 58.688], 
   # "Factions": [
   # {"Allegiance": "Empire", "FactionState": "None", "Influence": 0.174, "Name": "Traditional Eburnakura Nationalists", "Government": "Dictatorship"}, 
   # {"Allegiance": "Empire", "FactionState": "None", "Influence": 0.086, "Name": "Dukes of Lepchaimyu", "Government": "Feudal"}, 
   # {"Allegiance": "Empire", "FactionState": "None", "Influence": 0.099, "Name": "Apino Commodities", "Government": "Corporate"}, 
   # {"Allegiance": "Independent", "FactionState": "None", "Influence": 0.421, "Name": "Party of Yoru", "Government": "Dictatorship"}, 
   # {"Allegiance": "Empire", "FactionState": "None", "Influence": 0.171, "Name": "Maityan Constitution Party", "Government": "Dictatorship"}, 
   # {"Allegiance": "Independent", "FactionState": "Boom", "Influence": 0.049, "Name": "Lepchaimyu Gold Fortune Company", "Government": "Corporate"}],
   #  "Powers": ["Zemina Torval"], "event": "FSDJump", "SystemGovernment": "$government_Dictatorship;"}}"