# -*- coding: utf-8 -*-
# Generated by Django 1.11 on 2017-04-18 12:24
from __future__ import unicode_literals

from django.db import migrations


class Migration(migrations.Migration):

    dependencies = [
        ('app', '0001_initial'),
    ]

    operations = [
        migrations.RenameModel(
            old_name='Economies',
            new_name='Economy',
        ),
        migrations.RenameModel(
            old_name='Powers',
            new_name='Power',
        ),
        migrations.RenameModel(
            old_name='PowerStates',
            new_name='PowerState',
        ),
        migrations.RenameModel(
            old_name='Reserves',
            new_name='Reserve',
        ),
        migrations.RenameModel(
            old_name='SystemFactions',
            new_name='SystemFaction',
        ),
    ]
