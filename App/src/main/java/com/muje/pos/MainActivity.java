package com.muje.pos;

import java.util.Locale;

import android.app.Activity;
import android.app.ActionBar;
import android.app.Fragment;
import android.app.FragmentManager;
import android.app.FragmentTransaction;
import android.content.Intent;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.support.v13.app.FragmentPagerAdapter;
import android.os.Bundle;
import android.support.v4.view.ViewPager;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.TextView;

public class MainActivity extends Activity implements ActionBar.TabListener {

    private static final String TAB_KEY_INDEX = "tab_key";
    public static final int RESULT_SETTINGS = 100;

    /**
     * The {@link android.support.v4.view.PagerAdapter} that will provide
     * fragments for each of the sections. We use a
     * {@link FragmentPagerAdapter} derivative, which will keep every
     * loaded fragment in memory. If this becomes too memory intensive, it
     * may be best to switch to a
     * {@link android.support.v13.app.FragmentStatePagerAdapter}.
     */
    SectionsPagerAdapter mSectionsPagerAdapter;

    /**
     * The {@link ViewPager} that will host the section contents.
     */
    ViewPager mViewPager;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        SharedPreferences sharedPreferences = PreferenceManager.getDefaultSharedPreferences(this);
        String path = sharedPreferences.getString("prefDatabase", getResources().getString(R.string.database_path));
        Log.d("DEBUG", "Database path: " + path);

        // Set up PosReader with database path
        PosReader reader = PosReader.getInstance();
        reader.setDatabasePath(path);

        // Set up the action bar.
        final ActionBar actionBar = getActionBar();
        actionBar.setNavigationMode(ActionBar.NAVIGATION_MODE_TABS);

        // Create the adapter that will return a fragment for each of the three
        // primary sections of the activity.
        mSectionsPagerAdapter = new SectionsPagerAdapter(getFragmentManager());

        // Set up the ViewPager with the sections adapter.
        mViewPager = (ViewPager) findViewById(R.id.pager);
        mViewPager.setAdapter(mSectionsPagerAdapter);

        // When swiping between different sections, select the corresponding
        // tab. We can also use ActionBar.Tab#select() to do this if we have
        // a reference to the Tab.
        mViewPager.setOnPageChangeListener(new ViewPager.SimpleOnPageChangeListener() {
            @Override
            public void onPageSelected(int position) {
                actionBar.setSelectedNavigationItem(position);
            }
        });

        // For each of the sections in the app, add a tab to the action bar.
        for (int i = 0; i < mSectionsPagerAdapter.getCount(); i++) {
            // Create a tab with text corresponding to the page title defined by
            // the adapter. Also specify this Activity object, which implements
            // the TabListener interface, as the callback (listener) for when
            // this tab is selected.
            actionBar.addTab(
                    actionBar.newTab()
                            .setText(mSectionsPagerAdapter.getPageTitle(i))
                            .setTabListener(this));
        }

        if(savedInstanceState != null) {
            actionBar.setSelectedNavigationItem(savedInstanceState.getInt(TAB_KEY_INDEX,0));
        }
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        switch (item.getItemId()) {
            case R.id.action_refresh:
                Log.d("DEBUG", "Current Tab: " + Integer.toString(mViewPager.getCurrentItem()));
                View view = mViewPager.getChildAt(mViewPager.getCurrentItem());

                // Refresh database
                SharedPreferences sharedPreferences = PreferenceManager.getDefaultSharedPreferences(this);
                String path = sharedPreferences.getString("prefDatabase", getResources().getString(R.string.database_path));
                PosReader reader = PosReader.getInstance();
                reader.setDatabasePath(path);

                // reset and bind again
                ListView listView0 = (ListView)view.findViewById(R.id.listView);
                if(listView0.getAdapter().getClass() == SalesAdapter.class) {
                    SalesAdapter adapter = (SalesAdapter)listView0.getAdapter();
                    if(adapter != null) adapter.clear();

                    // rebind
                    adapter = new SalesAdapter(view.getContext(), reader.getSales());
                    listView0.setAdapter(adapter);
                } else if(listView0.getAdapter().getClass() == MonthAdapter.class) {
                    MonthAdapter adapter = (MonthAdapter)listView0.getAdapter();
                    if(adapter != null) adapter.clear();

                    // rebind
                    adapter = new MonthAdapter(view.getContext(), reader.getMonthlySales());
                    listView0.setAdapter(adapter);
                } else if(listView0.getAdapter().getClass() == MenuAdapter.class) {
                    MenuAdapter adapter = (MenuAdapter)listView0.getAdapter();
                    if(adapter != null) adapter.clear();

                    // rebind
                    adapter = new MenuAdapter(view.getContext(), reader.getCounter());
                    listView0.setAdapter(adapter);
                }

                return true;
            case R.id.action_settings:
                Log.i("INFO", "MainActivity.setting");
                Intent i = new Intent(this, SettingsActivity.class);
                startActivityForResult(i, RESULT_SETTINGS);
                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onTabSelected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) {
        // When the given tab is selected, switch to the corresponding page in
        // the ViewPager.
        mViewPager.setCurrentItem(tab.getPosition());
    }

    @Override
    public void onTabUnselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) {
    }

    @Override
    public void onTabReselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) {
    }

    /**
     * A {@link FragmentPagerAdapter} that returns a fragment corresponding to
     * one of the sections/tabs/pages.
     */
    public class SectionsPagerAdapter extends FragmentPagerAdapter {

        public SectionsPagerAdapter(FragmentManager fm) {
            super(fm);
        }

        @Override
        public Fragment getItem(int position) {
            // getItem is called to instantiate the fragment for the given page.
            switch(position) {
                case 0:
                    Fragment dayFragment = new DayFragment();
                    return dayFragment;
                case 1:
                    Fragment monthFragment = new MonthFragment();
                    return monthFragment;
                case 2:
                    Fragment menuFragment = new MenuFragment();
                    return menuFragment;
            }

            return null;
        }

        @Override
        public int getCount() {
            // Show 3 total pages.
            return 3;
        }

        @Override
        public CharSequence getPageTitle(int position) {
            Locale l = Locale.getDefault();
            switch (position) {
                case 0:
                    return getString(R.string.title_section1).toUpperCase(l);
                case 1:
                    return getString(R.string.title_section2).toUpperCase(l);
                case 2:
                    return getString(R.string.title_section3).toUpperCase(l);
            }
            return null;
        }
    }

    // onSaveInstanceState() is used to "remember" the current state when a
    // configuration change occurs such screen orientation change. This
    // is not meant for "long term persistence". We store the tab navigation
    @Override
    protected void onSaveInstanceState(Bundle outState) {
        super.onSaveInstanceState(outState);
        outState.putInt(TAB_KEY_INDEX, getActionBar().getSelectedNavigationIndex());

    }

}
