# -*- coding: utf-8 -*-
# Generated by Django 1.11 on 2017-04-25 12:37
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('app', '0005_auto_20170425_1330'),
    ]

    operations = [
        migrations.AlterField(
            model_name='fstate',
            name='description',
            field=models.TextField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name='govtype',
            name='description',
            field=models.TextField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name='sstate',
            name='description',
            field=models.TextField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name='superpower',
            name='description',
            field=models.TextField(blank=True, null=True),
        ),
    ]
