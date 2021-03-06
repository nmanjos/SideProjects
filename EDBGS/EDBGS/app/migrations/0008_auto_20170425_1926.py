# -*- coding: utf-8 -*-
# Generated by Django 1.11 on 2017-04-25 18:26
from __future__ import unicode_literals

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    dependencies = [
        ('app', '0007_auto_20170425_1627'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='sfhist',
            name='sFaction',
        ),
        migrations.AddField(
            model_name='sfhist',
            name='sControFaction',
            field=models.BooleanField(default=False, verbose_name='Controling_Faction'),
        ),
        migrations.AlterField(
            model_name='faction',
            name='allegiance',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.SuperPower'),
        ),
        migrations.AlterField(
            model_name='faction',
            name='government',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.GovType'),
        ),
        migrations.AlterField(
            model_name='faction',
            name='home_system',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.System'),
        ),
        migrations.AlterField(
            model_name='faction',
            name='state',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.fState'),
        ),
        migrations.AlterField(
            model_name='faction',
            name='updated_at',
            field=models.DateTimeField(null=True),
        ),
        migrations.AlterField(
            model_name='sfhist',
            name='fInfluence',
            field=models.FloatField(null=True, verbose_name='Influence'),
        ),
        migrations.AlterField(
            model_name='sfhist',
            name='fhState',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.fState'),
        ),
        migrations.AlterField(
            model_name='sfhist',
            name='sGovtype',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.GovType'),
        ),
        migrations.AlterField(
            model_name='sfhist',
            name='sPopulation',
            field=models.IntegerField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name='sfhist',
            name='sSecurity',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.Security'),
        ),
        migrations.AlterField(
            model_name='sfhist',
            name='seconomy',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.Economy'),
        ),
        migrations.AlterField(
            model_name='sfhist',
            name='shState',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.sState'),
        ),
        migrations.AlterField(
            model_name='sfhist',
            name='spower',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.Power'),
        ),
        migrations.AlterField(
            model_name='sfhist',
            name='spower_state',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.PowerState'),
        ),
        migrations.AlterField(
            model_name='system',
            name='govtype',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.GovType'),
        ),
        migrations.AlterField(
            model_name='system',
            name='population',
            field=models.IntegerField(null=True),
        ),
        migrations.AlterField(
            model_name='system',
            name='power',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.Power'),
        ),
        migrations.AlterField(
            model_name='system',
            name='power_state',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.PowerState'),
        ),
        migrations.AlterField(
            model_name='system',
            name='primary_economy_id',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.Economy'),
        ),
        migrations.AlterField(
            model_name='system',
            name='reserves',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.Reserve'),
        ),
        migrations.AlterField(
            model_name='system',
            name='security',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.Security'),
        ),
        migrations.AlterField(
            model_name='system',
            name='state',
            field=models.ForeignKey(null=True, on_delete=django.db.models.deletion.PROTECT, to='app.sState'),
        ),
        migrations.AlterField(
            model_name='system',
            name='updated_at',
            field=models.DateTimeField(null=True),
        ),
    ]
